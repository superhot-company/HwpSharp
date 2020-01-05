using System;
using System.IO;
using System.Text;
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
            get;
            private set;
        }

        /// <summary>
        /// Gets a preview text.
        /// </summary>
        public string PreviewText
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a preview image.
        /// </summary>
        public PreviewImage PreviewImage
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets document options
        /// </summary>
        public DocumentOption DocumentOption
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets scripts
        /// </summary>
        public Script Script
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets XML Templates
        /// </summary>
        public XmlTemplate XmlTemplate
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a document history
        /// </summary>
        public DocumentHistory DocumentHistory
        {
            get;
            private set;
        }

        private void Load(CompoundFile compoundFile)
        {
            FileHeader = LoadFileHeader(compoundFile);

            if (FileHeader.EncryptedByPassword)
            {
                throw new HwpUnsupportedFormatException("Does not support a password encrypted document.");
            }

            DocumentInformation = LoadDocumentInformation(compoundFile, FileHeader);
            BodyText = LoadBodyText(compoundFile, FileHeader, DocumentInformation);
            BinaryData = LoadBinaryData(compoundFile, FileHeader, DocumentInformation);
            PreviewText = LoadPreviewText(compoundFile);
            PreviewImage = LoadPreviewImage(compoundFile);
            DocumentOption = LoadDocumentOption(compoundFile);
            Script = LoadScript(compoundFile, FileHeader, DocumentInformation);
            XmlTemplate = LoadXmlTemplate(compoundFile);
            DocumentHistory = LoadDocumentHistory(compoundFile, FileHeader, DocumentInformation);
        }

        private DocumentHistory LoadDocumentHistory(CompoundFile compoundFile, FileHeader fileHeader, DocumentInformation documentInformation)
        {
            var docHistory = new DocumentHistory();
            try
            {
                var storage = compoundFile.RootStorage.TryGetStorage("DocHistory");
                if (storage == null)
                {
                    return null;
                }

                storage.VisitEntries(item =>
                {
                    var data = (item as CFStream).GetData();

                    using (var reader = new HwpReader(data, fileHeader.Distributed, fileHeader.Compressed))
                    {
                        docHistory.Streams[item.Name] = reader.ReadBytes((int)reader.BaseStream.Length);
                    }
                }, false);
            }
            catch (CFItemNotFound exception)
            {
                throw new HwpFileFormatException("Specified document does not have DocHistory storage.", exception);
            }

            return docHistory;
        }

        private static XmlTemplate LoadXmlTemplate(CompoundFile compoundFile)
        {
            var xmlTemplate = new XmlTemplate();
            try
            {
                var storage = compoundFile.RootStorage.TryGetStorage("XMLTemplate");
                if (storage == null)
                {
                    return null;
                }

                storage.VisitEntries(item =>
                {
                    xmlTemplate.Streams[item.Name] = (item as CFStream).GetData();
                }, false);
            }
            catch (CFItemNotFound exception)
            {
                throw new HwpFileFormatException("Specified document does not have XMLTemplate storage.", exception);
            }

            return xmlTemplate;
        }

        private static Script LoadScript(CompoundFile compoundFile, FileHeader fileHeader, DocumentInformation documentInformation)
        {
            var script = new Script();
            try
            {
                var storage = compoundFile.RootStorage.TryGetStorage("Scripts");
                if (storage == null)
                {
                    return null;
                }

                storage.VisitEntries(item =>
                {
                    var data = (item as CFStream).GetData();
                    using (var reader = new HwpReader(data, fileHeader.Distributed, false))
                    {
                        script.Streams[item.Name] = reader.ReadBytes((int)reader.BaseStream.Length);
                    }
                }, false);
            }
            catch (CFItemNotFound exception)
            {
                throw new HwpFileFormatException("Specified document does not have Scripts storage.", exception);
            }

            return script;
        }

        private static DocumentOption LoadDocumentOption(CompoundFile compoundFile)
        {
            var docOption = new DocumentOption();
            try
            {
                var storage = compoundFile.RootStorage.TryGetStorage("DocOptions");
                if (storage == null)
                {
                    return null;
                }

                storage.VisitEntries(item =>
                {
                    docOption.Streams[item.Name] = (item as CFStream).GetData();
                }, false);
            }
            catch (CFItemNotFound exception)
            {
                throw new HwpFileFormatException("Specified document does not have DocOptions storage.", exception);
            }

            return docOption;
        }

        private static PreviewImage LoadPreviewImage(CompoundFile compoundFile)
        {
            byte[] data;
            try
            {
                var stream = compoundFile.RootStorage.GetStream("PrvImage");
                data = stream.GetData();
            }
            catch (CFItemNotFound exception)
            {
                throw new HwpFileFormatException("Specified document does not have the PrvImage stream.", exception);
            }
            return new PreviewImage(data);
        }

        private static string LoadPreviewText(CompoundFile compoundFile)
        {
            byte[] data;
            try
            {
                var stream = compoundFile.RootStorage.GetStream("PrvText");
                data = stream.GetData();
            }
            catch (CFItemNotFound exception)
            {
                throw new HwpFileFormatException("Specified document does not have the PrvText stream.", exception);
            }
            return Encoding.Unicode.GetString(data);
        }

        private static BinaryData LoadBinaryData(CompoundFile compoundFile, FileHeader fileHeader, DocumentInformation docInfo)
        {
            var binData = new BinaryData(fileHeader, docInfo);
            try
            {
                var storage = compoundFile.RootStorage.TryGetStorage("BinData");
                if (storage == null)
                {
                    return null;
                }

                foreach(var record in docInfo.BinDataList)
                {
                    if (record.Property.Type == DataRecords.BinData.TypeProperty.Embedding || record.Property.Type == DataRecords.BinData.TypeProperty.Storage)
                    {
                        var id = record.BinaryDataId;

                        byte[] data;
                        try
                        {
                            // BIN0001.bmp
                            var stream = storage.GetStream(string.Format("BIN{0,4:X4}.", id) + record.BinaryDataExtension);
                            data = stream.GetData();
                        }
                        catch (CFItemNotFound exception)
                        {
                            throw new HwpCorruptedBodyTextException("The document does not have some binary data. File may be corrupted.", exception);
                        }

                        var compressed = record.Property.Compression == DataRecords.BinData.CompressionProperty.StorageDefault ? fileHeader.Compressed : record.Property.Compression == DataRecords.BinData.CompressionProperty.Compress ? true : record.Property.Compression == DataRecords.BinData.CompressionProperty.NotCompress ? false : throw new HwpCorruptedDataRecordException();
                        using(var reader = new HwpReader(data, compressed: compressed))
                        {
                            var bytes = reader.ReadBytes((int)reader.BaseStream.Length);
                            binData.Data[id] = bytes;
                        }
                    }
                }
            }
            catch (CFItemNotFound exception)
            {
                throw new HwpFileFormatException("Specified document does not have any BinData storage.", exception);
            }

            return binData;
        }

        private static BodyText LoadBodyText(CompoundFile compoundFile, FileHeader fileHeader, DocumentInformation docInfo)
        {
            var bodyText = new BodyText(fileHeader, docInfo);
            try
            {
                var storage = !fileHeader.Distributed ? compoundFile.RootStorage.GetStorage("BodyText") : compoundFile.RootStorage.GetStorage("ViewText");

                for (var i = 0; i < docInfo.DocumentProperty.SectionCount; ++i)
                {
                    byte[] data;
                    try
                    {
                        var stream = storage.GetStream($"Section{i}");
                        data = stream.GetData();
                    }
                    catch (CFItemNotFound exception)
                    {
                        throw new HwpCorruptedBodyTextException("The document does not have some sections. File may be corrupted.", exception);
                    }

                    using(var reader = new HwpReader(data, fileHeader.Distributed, fileHeader.Compressed))
                    {
                        var section = new BodyText.Section(reader, fileHeader, docInfo);

                        bodyText.Sections.Add(section);
                    }
                }
            }
            catch (CFItemNotFound exception)
            {
                throw new HwpFileFormatException("Specified document does not have BodyText storage.", exception);
            }

            return bodyText;
        }

        private static DocumentInformation LoadDocumentInformation(CompoundFile compoundFile, FileHeader fileHeader)
        {
            byte[] data;
            try
            {
                var stream = compoundFile.RootStorage.GetStream("DocInfo");
                data = stream.GetData();
            }
            catch (CFItemNotFound exception)
            {
                throw new HwpFileFormatException("Specified document does not have a DocInfo stream.", exception);
            }

            using (var reader = new HwpReader(data, compressed: fileHeader.Compressed))
            {
                var docInfo = new DocumentInformation(reader, fileHeader);

                return docInfo;
            }
        }

        private static FileHeader LoadFileHeader(CompoundFile compoundFile)
        {
            byte[] data;
            try
            {
                var stream = compoundFile.RootStorage.GetStream("FileHeader");
                data = stream.GetData();
            }
            catch (CFItemNotFound exception)
            {
                throw new HwpFileFormatException("Specified document does not have a FileHeader stream.", exception);
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
                using (var compoundFile = new CompoundFile(stream, CFSUpdateMode.ReadOnly, CFSConfiguration.EraseFreeSectors | CFSConfiguration.SectorRecycle))
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
                using (var compoundFile = new CompoundFile(filename, CFSUpdateMode.ReadOnly, CFSConfiguration.EraseFreeSectors | CFSConfiguration.SectorRecycle))
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
