using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashSlingerCore
{
    public enum HasherEventReportType
    {
        ProgressReport = 0,
        Completed,
        Error
    }

    public class HasherEventArgs : EventArgs
    {
        #region Ctor

        public HasherEventArgs(HasherEventReportType reportType,
                               int? streamCount,
                               int? streamCurrent,
                               long? processedBytes,
                               long? totalBytes)
        {
            // Make sure we didn't get handed some
            // bullshit parameter values on a platter
            if (streamCount != null && streamCount <= 0) throw new ArgumentOutOfRangeException("streamCount", String.Format(exceptionMessageArgument, streamCount));
            if (streamCurrent!= null && streamCurrent <= 0) throw new ArgumentOutOfRangeException("streamCurrent", String.Format(exceptionMessageArgument, streamCurrent));
            if (processedBytes != null && processedBytes <= 0) throw new ArgumentOutOfRangeException("processedBytes", String.Format(exceptionMessageArgument, streamCurrent));
            if (totalBytes != null && totalBytes <= 0) throw new ArgumentOutOfRangeException("totalBytes", String.Format(exceptionMessageArgument, totalBytes));

            this.eventReportType = reportType;
            this.totalStreamCount = streamCount;
            this.currentStreamNumber = streamCurrent;
            this.streamBytesProcessed = processedBytes;
            this.streamTotalBytes = totalBytes;
        }

        #endregion

        const string exceptionMessageArgument = "Argument must be greater than zero. Passed value is {0}.";
        
        #region Public Properties

        private HasherEventReportType eventReportType;
        public HasherEventReportType ReportType
        {
            get { return this.eventReportType; }
        }

        private int? totalStreamCount = 0;
        public int? StreamCount
        {
            get { return this.totalStreamCount; }
        }

        private int? currentStreamNumber = 0;
        public int? CurrentStream
        {
            get { return this.currentStreamNumber; }
        }

        public long? streamBytesProcessed = 0;
        public long? BytesProcessed
        {
            get { return this.streamBytesProcessed; }
        }

        public long? streamTotalBytes;
        public long? StreamBytes
        {
            get { return this.streamTotalBytes; }
        }

        #endregion
    }
}
