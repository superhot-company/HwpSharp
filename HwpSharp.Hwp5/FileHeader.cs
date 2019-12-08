using System;
using System.Linq;
using SuperHot.HwpSharp.Common;
using SuperHot.HwpSharp.Hwp5.HwpType;

namespace SuperHot.HwpSharp.Hwp5
{
    /// <summary>
    /// Represents a hwp 5.0 file header.
    /// </summary>
    public class FileHeader : IFileHeader
    {
        public const int FIleHeaderLength = 256;

        public const int SignatureLength = 32;
        public static readonly byte[] SignatureBytes =
        {
            0x48, 0x57, 0x50, 0x20, 0x44, 0x6f, 0x63, 0x75,
            0x6d, 0x65, 0x6e, 0x74, 0x20, 0x46, 0x69, 0x6c,
            0x65, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        }; // "HWP Document File"

        private uint _attribute;
        private uint _attribute2;

        /// <summary>
        /// Gets or sets the document file version.
        /// </summary>
        public Version FileVersion { get; set; }
        
        /// <summary>
        /// Gets or sets the compressed attribute.
        /// </summary>
        public bool Compressed
        {
            get
            {
                return (_attribute & (1u << 0)) != 0;
            }
            set
            {
                _attribute = (_attribute & ~(1u << 0)) | ((value ? 1u : 0u) << 0);
            }
        }

        /// <summary>
        /// Gets or sets the encryption attribute.
        /// </summary>
        public bool PasswordEncrypted
        {
            get
            {
                return (_attribute & (1u << 1)) != 0;
            }
            set
            {
                _attribute = (_attribute & ~(1u << 1)) | ((value ? 1u : 0u) << 1);
            }
        }

        /// <summary>
        /// Gets or sets the published document attribute.
        /// </summary>
        public bool Published
        {
            get
            {
                return (_attribute & (1u << 2)) != 0;
            }
            set
            {
                _attribute = (_attribute & ~(1u << 2)) | ((value ? 1u : 0u) << 2);
            }
        }

        /// <summary>
        /// Gets or sets whether the document has scripts.
        /// </summary>
        public bool HasScript
        {
            get
            {
                return (_attribute & (1u << 3)) != 0;
            }
            set
            {
                _attribute = (_attribute & ~(1u << 3)) | ((value ? 1u : 0u) << 3);
            }
        }

        /// <summary>
        /// Gets or sets the DRM secure attribute.
        /// </summary>
        public bool DrmSecured
        {
            get
            {
                return (_attribute & (1u << 4)) != 0;
            }
            set
            {
                _attribute = (_attribute & ~(1u << 4)) | ((value ? 1u : 0u) << 4);
            }
        }

        /// <summary>
        /// Gets or sets whether the document has XML template storages.
        /// </summary>
        public bool HasXmlTemplateStorage
        {
            get
            {
                return (_attribute & (1u << 5)) != 0;
            }
            set
            {
                _attribute = (_attribute & ~(1u << 5)) | ((value ? 1u : 0u) << 5);
            }
        }

        /// <summary>
        /// Gets or sets whether the document history is managed.
        /// </summary>
        public bool HasHistory
        {
            get
            {
                return (_attribute & (1u << 6)) != 0;
            }
            set
            {
                _attribute = (_attribute & ~(1u << 6)) | ((value ? 1u : 0u) << 6);
            }
        }

        /// <summary>
        /// Gets or sets whether the document has a digital sign.
        /// </summary>
        public bool HasSign
        {
            get
            {
                return (_attribute & (1u << 7)) != 0;
            }
            set
            {
                _attribute = (_attribute & ~(1u << 7)) | ((value ? 1u : 0u) << 7);
            }
        }

        /// <summary>
        /// Gets or sets the certificate encryption attribute.
        /// </summary>
        public bool CertificateEncrypted
        {
            get
            {
                return (_attribute & (1u << 8)) != 0;
            }
            set
            {
                _attribute = (_attribute & ~(1u << 8)) | ((value ? 1u : 0u) << 8);
            }
        }

        /// <summary>
        /// Gets or sets whether the additional certificate is stored.
        /// </summary>
        public bool CertificateReserved
        {
            get
            {
                return (_attribute & (1u << 9)) != 0;
            }
            set
            {
                _attribute = (_attribute & ~(1u << 9)) | ((value ? 1u : 0u) << 9);
            }
        }

        /// <summary>
        /// Gets or sets whether the document is encrypted with certificate and DRM.
        /// </summary>
        public bool CertificateDrmSecured
        {
            get
            {
                return (_attribute & (1u << 10)) != 0;
            }
            set
            {
                _attribute = (_attribute & ~(1u << 10)) | ((value ? 1u : 0u) << 10);
            }
        }

        /// <summary>
        /// Gets or sets the CCL document attribute.
        /// </summary>
        public bool CclDocumented
        {
            get
            {
                return (_attribute & (1u << 11)) != 0;
            }
            set
            {
                _attribute = (_attribute & ~(1u << 11)) | ((value ? 1u : 0u) << 11);
            }
        }

        public bool MobileOptimized
        {
            get
            {
                return (_attribute & (1u << 12)) != 0;
            }
            set
            {
                _attribute = (_attribute & ~(1u << 12)) | ((value ? 1u : 0u) << 12);
            }
        }

        public bool PersonalInformationProtected
        {
            get
            {
                return (_attribute & (1u << 13)) != 0;
            }
            set
            {
                _attribute = (_attribute & ~(1u << 13)) | ((value ? 1u : 0u) << 13);
            }
        }

        public bool TrackChange
        {
            get
            {
                return (_attribute & (1u << 14)) != 0;
            }
            set
            {
                _attribute = (_attribute & ~(1u << 14)) | ((value ? 1u : 0u) << 14);
            }
        }

        public bool KoglDocument
        {
            get
            {
                return (_attribute & (1u << 15)) != 0;
            }
            set
            {
                _attribute = (_attribute & ~(1u << 15)) | ((value ? 1u : 0u) << 15);
            }
        }

        public bool HasVideoControl
        {
            get
            {
                return (_attribute & (1u << 16)) != 0;
            }
            set
            {
                _attribute = (_attribute & ~(1u << 16)) | ((value ? 1u : 0u) << 16);
            }
        }

        public bool HasIndexFieldControl
        {
            get
            {
                return (_attribute & (1u << 17)) != 0;
            }
            set
            {
                _attribute = (_attribute & ~(1u << 17)) | ((value ? 1u : 0u) << 17);
            }
        }

        public bool HasLicenseInfo
        {
            get
            {
                return (_attribute2 & (1u << 0)) != 0;
            }
            set
            {
                _attribute2 = (_attribute2 & ~(1u << 0)) | ((value ? 1u : 0u) << 0);
            }
        }

        public bool CopyProtectedByLicense
        {
            get
            {
                return (_attribute2 & (1u << 1)) != 0;
            }
            set
            {
                _attribute2 = (_attribute2 & ~(1u << 1)) | ((value ? 1u : 0u) << 1);
            }
        }

        public bool CopyOnSameCondition
        {
            get
            {
                return (_attribute2 & (1u << 2)) != 0;
            }
            set
            {
                _attribute2 = (_attribute2 & ~(1u << 2)) | ((value ? 1u : 0u) << 2);
            }
        }

        public uint EncryptVersion
        {
            get; set;
        }

        public byte KoglLicenseCountry
        {
            get; set;
        }

        public byte[] ReservedBytes
        {
            get; set;
        }

        /// <summary>
        /// Creates a <see cref="FileHeader"/> instance with a blank setting.
        /// </summary>
        public FileHeader()
        {
            FileVersion = new Version(5, 0, 0, 0);
        }

        public FileHeader(HwpReader stream)
        {
            SetFileHeader(stream);
        }

        private void SetFileHeader(HwpReader stream)
        {
            ParseSignature(stream);
            ParseFileVersion(stream);
            ParseAttribute(stream);
            ParseEncryptVersion(stream);
            ParseKoglLicenseCountry(stream);
            ParseReservedBytes(stream);
        }

        private void ParseReservedBytes(HwpReader stream)
        {
            ReservedBytes = stream.ReadBytes(207);
        }

        private void ParseKoglLicenseCountry(HwpReader stream)
        {
            KoglLicenseCountry = stream.ReadByte();
        }

        private void ParseEncryptVersion(HwpReader stream)
        {
            EncryptVersion = stream.ReadUInt32();
        }

        private void ParseAttribute(HwpReader stream)
        {
            _attribute = stream.ReadUInt32();
            _attribute2 = stream.ReadUInt32();
        }

        private void ParseFileVersion(HwpReader stream)
        {
            var versionBytes = stream.ReadBytes(4);

            FileVersion = new Version(versionBytes[3], versionBytes[2], versionBytes[1], versionBytes[0]);

            if (FileVersion.Major != 5)
            {
                throw new HwpUnsupportedFormatException($"File version '{FileVersion}' is not imcompatible.");
            }
        }

        private static void ParseSignature(HwpReader stream)
        {
            var signature = stream.ReadBytes(SignatureLength);

            if (!SignatureBytes.SequenceEqual(signature))
            {
                throw new HwpFileFormatException("Signature is not matched.");
            }
        }
    }
}
