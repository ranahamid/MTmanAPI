using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using P23.MetaTrader4.Manager;
using P23.MetaTrader4.Manager.Contracts;

namespace MTmanAPI
{
    public class PumpingDataProvider 
    {
        //private readonly ConnectionParameters _connectionParameters;

        public event EventHandler PumpingStarted;
        public event EventHandler PumpingStopped;
        public ClrWrapper clrWrapper;



        private bool _disposed;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private Task _healtChecker;

        public PumpingDataProvider( )
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;
           
            _healtChecker = Task.Run(async () =>
            {
                while (!_disposed)
                {
                    try
                    {
                        try
                        {
                            HealthCheck();
                        }
                        catch (Exception)
                        {
                            Task.Delay(new TimeSpan(0, 15, 0), token).Wait();
                            ResetConnection();
                        }
                    }
                    catch (Exception exception)
                    {
                        OnHealthCheckException(exception);
                    }
                    await Task.Delay(new TimeSpan(0, 0, 15), token);
                }
            }, token);
        }

        private void HealthCheck()
        {
            var users = clrWrapper != null ? clrWrapper.UsersGet() : new List<UserRecord>();
            if (users.Count == 0)
                ResetConnection();
        }

        public void ConnectPumping()
        {
            clrWrapper = ConnectionMT4Server.clrWrapper;
            SubscribeEvents();
            clrWrapper.PumpingSwitchEx(PumpingMode.Default);
        }

        private void SubscribeEvents()
        {
            clrWrapper.PumpingStopped += MetaTraderWrapperOnPumpingStopped;
            clrWrapper.PumpingStarted += MetaTraderWrapperOnPumpingStarted;
        }

        private void MetaTraderWrapperOnPumpingStarted(object sender, EventArgs eventArgs)
        {
            OnPumpingStarted();
        }

        private void MetaTraderWrapperOnPumpingStopped(object sender, EventArgs eventArgs)
        {
            if (!_disposed)
            {
                ResetConnection();
                OnPumpingStopped();
            }
        }

        private void ResetConnection()
        {
            ClearPreviousState();
            ConnectPumping();
        }

        private void ClearPreviousState()
        {
            if (clrWrapper != null)
            {
                clrWrapper.PumpingStopped -= MetaTraderWrapperOnPumpingStopped;
                clrWrapper.PumpingStarted -= MetaTraderWrapperOnPumpingStarted;
                clrWrapper.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            _cancellationTokenSource.Cancel();
            if (disposing)
            {
                // Free any other managed objects here. 
                //
            }
            ClearPreviousState();
            _disposed = true;
        }

        ~PumpingDataProvider()
        {
            Dispose(false);
        }

        protected virtual void OnPumpingStarted()
        {
            PumpingStarted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPumpingStopped()
        {
            PumpingStopped?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnHealthCheckException(Exception exception)
        {
           /* var handler = HealthCheckException;
            if (handler != null) handler(this, new ErrorEventArgs(exception));*/
        }
    }
}
