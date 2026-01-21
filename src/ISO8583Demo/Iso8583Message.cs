using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISO8583Demo
{
    /// <summary>
    /// Simple ISO8583 Message implementation for demonstration purposes
    /// </summary>
    public class Iso8583Message
    {
        private readonly Dictionary<int, string> _dataElements = new Dictionary<int, string>();
        
        public string MessageType { get; set; }
        
        /// <summary>
        /// Set or get data element by field number
        /// </summary>
        public string this[int fieldNumber]
        {
            get => _dataElements.ContainsKey(fieldNumber) ? _dataElements[fieldNumber] : null;
            set => _dataElements[fieldNumber] = value;
        }
        
        /// <summary>
        /// Generate bitmap indicating which fields are present
        /// </summary>
        private byte[] GenerateBitmap()
        {
            // For simplicity, using 64-bit bitmap (fields 1-64)
            byte[] bitmap = new byte[8];
            
            foreach (var field in _dataElements.Keys.OrderBy(k => k))
            {
                if (field >= 1 && field <= 64)
                {
                    int byteIndex = (field - 1) / 8;
                    int bitIndex = 7 - ((field - 1) % 8);
                    bitmap[byteIndex] |= (byte)(1 << bitIndex);
                }
            }
            
            return bitmap;
        }
        
        /// <summary>
        /// Convert message to byte array for transmission
        /// </summary>
        public byte[] ToByteArray()
        {
            if (string.IsNullOrEmpty(MessageType))
                throw new InvalidOperationException("MessageType must be set");
                
            List<byte> result = new List<byte>();
            
            // Add message type (4 bytes ASCII)
            result.AddRange(Encoding.ASCII.GetBytes(MessageType));
            
            // Add bitmap
            result.AddRange(GenerateBitmap());
            
            // Add data elements in order
            foreach (var field in _dataElements.Keys.OrderBy(k => k))
            {
                if (!string.IsNullOrEmpty(_dataElements[field]))
                {
                    result.AddRange(Encoding.ASCII.GetBytes(_dataElements[field]));
                }
            }
            
            return result.ToArray();
        }
        
        /// <summary>
        /// Parse bitmap and return list of present fields
        /// </summary>
        public static List<int> ParseBitmap(byte[] bitmap)
        {
            List<int> fields = new List<int>();
            
            for (int byteIndex = 0; byteIndex < Math.Min(bitmap.Length, 8); byteIndex++)
            {
                for (int bitIndex = 7; bitIndex >= 0; bitIndex--)
                {
                    if ((bitmap[byteIndex] & (1 << bitIndex)) != 0)
                    {
                        fields.Add((byteIndex * 8) + (7 - bitIndex) + 1);
                    }
                }
            }
            
            return fields;
        }
        
        /// <summary>
        /// Get a human-readable representation of the message
        /// </summary>
        public string ToReadableString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Message Type: {MessageType}");
            sb.AppendLine("Data Elements:");
            
            foreach (var kvp in _dataElements.OrderBy(k => k.Key))
            {
                sb.AppendLine($"  Field {kvp.Key}: {kvp.Value}");
            }
            
            return sb.ToString();
        }
    }
}