using System;
using System.Runtime.Serialization;

namespace SuperHot.HwpSharp.Common
{
    public abstract class HwpException : Exception
    {
        protected HwpException()
        {
        }

        protected HwpException(string message) : base(message)
        {
        }

        protected HwpException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
    /// <summary>
    /// The exception that is thrown when a file or stream does not contains a HWP document, or HwpSharp cannot recognized a HWP document.
    /// </summary>
    public class HwpFileFormatException : HwpException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HwpFileFormatException"/> class.
        /// </summary>
        public HwpFileFormatException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HwpFileFormatException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public HwpFileFormatException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HwpFileFormatException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public HwpFileFormatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// The exception that is thrown when hwpSharp does not support this type of HWP document.
    /// </summary>
    public class HwpUnsupportedFormatException : HwpException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HwpUnsupportedFormatException"/> class.
        /// </summary>
        public HwpUnsupportedFormatException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HwpUnsupportedFormatException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public HwpUnsupportedFormatException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HwpUnsupportedFormatException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public HwpUnsupportedFormatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// The exception that is thrown when hwpSharp read a corrupted file.
    /// </summary>
    public class HwpCorruptedException : HwpException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HwpCorruptedException"/> class.
        /// </summary>
        public HwpCorruptedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HwpCorruptedException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public HwpCorruptedException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HwpCorruptedException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public HwpCorruptedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public class HwpCorruptedFileHeaderException : HwpCorruptedException
    {
        public HwpCorruptedFileHeaderException()
        {
        }

        public HwpCorruptedFileHeaderException(string message)
            : base(message)
        {
        }

        public HwpCorruptedFileHeaderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public class HwpCorruptedDocumentInformationException : HwpCorruptedException
    {
        public HwpCorruptedDocumentInformationException()
        {
        }

        public HwpCorruptedDocumentInformationException(string message)
            : base(message)
        {
        }

        public HwpCorruptedDocumentInformationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// The exception that is thrown when hwpSharp read a corrupted data record from stream.
    /// </summary>
    public class HwpCorruptedDataRecordException : HwpCorruptedException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HwpCorruptedDataRecordException"/> class.
        /// </summary>
        public HwpCorruptedDataRecordException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HwpCorruptedDataRecordException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public HwpCorruptedDataRecordException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HwpCorruptedDataRecordException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public HwpCorruptedDataRecordException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// The exception that is thrown when hwpSharp read a corrupted body text.
    /// </summary>
    public class HwpCorruptedBodyTextException : HwpCorruptedException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HwpCorruptedBodyTextException"/> class.
        /// </summary>
        public HwpCorruptedBodyTextException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HwpCorruptedBodyTextException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public HwpCorruptedBodyTextException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HwpCorruptedBodyTextException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public HwpCorruptedBodyTextException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    public class HwpDataRecordConstructorException : HwpException
    {
    }

    public class HwpUnsupportedPropertyException : HwpException
    {
    }
}
