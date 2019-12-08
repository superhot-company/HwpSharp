using System;
using System.IO;
using System.IO.Compression;
using OpenMcdf;
using SuperHot.HwpSharp.Common;

namespace SuperHot.HwpSharp.Hwp5
{
    /// <summary>
    /// Represents a hwp 5.0 document.
    /// </summary>
    public class Document : IHwpDocument
    {
        /// <summary>
        /// This document is a HWP 5.0 document.
        /// </summary>
        public string HwpVersion => "5.0";

        /// <summary>
        /// Gets a file recognition header of this document.
        /// </summary>
        public FileHeader FileHeader { get; private set; }

        /// <summary>
        /// Gets a document information of this document.
        /// </summary>
        public DocumentInformation DocumentInformation { get; private set; }

        /// <summary>
        /// Gets a body text of this document.
        /// </summary>
        public BodyText BodyText { get; private set; }

        /// <summary>
        /// Gets a summary information of this document.
        /// </summary>
        public SummaryInformation SummaryInformation
        {
            get { throw new NotImplementedException(); }
            private set { throw new NotImplementedException(); }
        }

        public BinaryData BinaryData
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a preview text.
        /// </summary>
        public PreviewText PreviewText
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a preview image.
        /// </summary>
        public PreviewImage PreviewImage
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets document options
        /// </summary>
        public DocumentOption DocumentOption
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets scripts
        /// </summary>
        public Script Script
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets XML Templates
        /// </summary>
        public XmlTemplate XmlTemplate
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets a document history
        /// </summary>
        public DocumentHistory DocumentHistory
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        internal Document(CompoundFile compoundFile)
        {
            Load(compoundFile);
        }

        private void Load(CompoundFile compoundFile)
        {
            FileHeader = LoadFileHeader(compoundFile);

            if (FileHeader.PasswordEncrypted)
            {
                throw new HwpUnsupportedFormatException("Does not support a password encrypted document.");
            }

            DocumentInformation = LoadDocumentInformation(compoundFile, FileHeader);
            BodyText = LoadBodyText(compoundFile, FileHeader, DocumentInformation);
        }

        private static BodyText LoadBodyText(CompoundFile compoundFile, FileHeader fileHeader, DocumentInformation docInfo)
        {
            CFStorage storage;
            var bodyText = new BodyText(fileHeader, docInfo);
            try
            {
                storage = !fileHeader.Published ? compoundFile.RootStorage.GetStorage("BodyText") : compoundFile.RootStorage.GetStorage("ViewText");

                for (var i = 0; i < docInfo.DocumentProperty.SectionCount; ++i)
                {
                    CFStream stream;
                    byte[] data;
                    try
                    {
                        stream = storage.GetStream($"Section{i}");
                        data = stream.GetData();
                    }
                    catch (CFItemNotFound exception)
                    {
                        throw new HwpCorruptedBodyTextException("The document does not have some sections. File may be corrupted.", exception);
                    }

                    using(var reader = new HwpReader(data, fileHeader.Published, fileHeader.Compressed))
                    {
                        var section = new BodyText.Section(reader, fileHeader, docInfo);

                        bodyText.Sections.Add(section);
                    }
                }
            }
            catch (CFItemNotFound exception)
            {
                throw new HwpFileFormatException("Specified document does not have any BodyText fields.", exception);
            }

            return bodyText;
        }

        private static DocumentInformation LoadDocumentInformation(CompoundFile compoundFile, FileHeader fileHeader)
        {
            CFStream stream;
            byte[] data;
            try
            {
                stream = compoundFile.RootStorage.GetStream("DocInfo");
                data = stream.GetData();
            }
            catch (CFItemNotFound exception)
            {
                throw new HwpFileFormatException("Specified document does not have a DocInfo field.", exception);
            }

            using (var reader = new HwpReader(data, compressed: fileHeader.Compressed))
            {
                var docInfo = new DocumentInformation(reader, fileHeader);

                return docInfo;
            }
        }

        private static FileHeader LoadFileHeader(CompoundFile compoundFile)
        {
            CFStream stream;
            byte[] data;
            try
            {
                stream = compoundFile.RootStorage.GetStream("FileHeader");
                data = stream.GetData();
            }
            catch (CFItemNotFound exception)
            {
                throw new HwpFileFormatException("Specified document does not have a FileHeader field.", exception);
            }

            if (data.Length != FileHeader.FIleHeaderLength)
            {
                throw new HwpFileFormatException("The length of a file header is not 256.");
            }

            using (var reader = new HwpReader(data)) {
                var fileHeader = new FileHeader(reader);

                return fileHeader;
            }
        }

        /// <summary>
        /// Creates a <see cref="Document"/> instance from a <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">A stream which contains a hwp 5 document.</param>
        public Document(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            try
            {
                using (var compoundFile = new CompoundFile(stream, CFSUpdateMode.Update, CFSConfiguration.EraseFreeSectors | CFSConfiguration.SectorRecycle))
                {
                    Load(compoundFile);
                }
            }
            catch(CFFileFormatException exception)
            {
                throw new HwpFileFormatException("Specified document is not a hwp 5 document format.", exception);
            }
        }

        /// <summary>
        /// Creates a <see cref="Document"/> instance from a file.
        /// </summary>
        /// <param name="filename">A file name of a hwp 5 document.</param>
        public Document(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            try
            {
                using (var compoundFile = new CompoundFile(filename, CFSUpdateMode.Update, CFSConfiguration.EraseFreeSectors | CFSConfiguration.SectorRecycle))
                {
                    Load(compoundFile);
                }
            }
            catch (CFFileFormatException exception)
            {
                throw new HwpFileFormatException("Specified document is not a hwp 5 document format.", exception);
            }
        }
    }
}
