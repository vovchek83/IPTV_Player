using System;
using System.IO;
using Caliburn.Micro;

namespace IPTV.Infrastructure
{
    public class FileLogger : ILog
    {
        #region Data Members

        private readonly Type _type;
        private readonly string _filename;

        #endregion Fields

        #region Constructors

        public FileLogger(Type type, Func<bool> canDeleteFile)
        {
            this._type = type;
            this._filename = string.Format("{0}.log", DateTime.Today.ToString("yyyy-MM-dd"));

            if (canDeleteFile() && File.Exists(this._filename))
            {
                File.Delete(this._filename);
            }
        }

        #endregion Constructors

        #region Methods

        public void Error(Exception exception)
        {
            this.WriteMessage(this.CreateLogMessage(exception.ToString()), "ERROR");
        }

        public void Info(string format, params object[] args)
        {
            this.WriteMessage(this.CreateLogMessage(format, args), "INFO");
        }

        public void Warn(string format, params object[] args)
        {
            this.WriteMessage(this.CreateLogMessage(format, args), "WARN");
        }

        private string CreateLogMessage(string format, params object[] args)
        {
            return string.Format(
                "[{0}] {1}",
                DateTime.Now.ToString("o"),
                string.Format(format, args));
        }

        /// <summary>
        /// Writes the message to the file
        /// </summary>
        private void WriteMessage(string message, string catgeory)
        {
            using (StreamWriter writer = new StreamWriter(this._filename, true))
            {
                writer.WriteLine("{0}: {1}", catgeory, message);
            }

            //  Trace.WriteLine( string.Format("{0}: {1}", catgeory, message));

        }

        #endregion Methods 
    }
}