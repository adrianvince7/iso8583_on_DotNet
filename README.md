# ISO8583 on .NET

Demonstrative project for the integration between an ISO8583 endpoint and a .NET application.

## Overview

This project demonstrates how to create, format, and work with ISO8583 messages in a .NET application. ISO8583 is the international standard for financial transaction card originated messages and is used by card payment systems worldwide.

## What's Included

- **ISO8583 Message Library**: A simple implementation of ISO8583 message creation and formatting
- **Demo Application**: Console application showing various message types:
  - Authorization Request (0200)
  - Authorization Response (0210)
  - Reversal Request (0400)
  - Network Management (0800)

## Getting Started

### Prerequisites

- .NET SDK 8.0 or later
- A code editor (Visual Studio, VS Code, or Rider)

### Building the Project

```bash
# Clone the repository
git clone https://github.com/adrianvince7/iso8583_on_DotNet.git
cd iso8583_on_DotNet

# Build the solution
dotnet build

# Run the demo
dotnet run --project src/ISO8583Demo/ISO8583Demo.csproj
```

## Project Structure

```
iso8583_on_DotNet/
├── src/
│   └── ISO8583Demo/
│       ├── Iso8583Message.cs    # ISO8583 message implementation
│       ├── Program.cs            # Demo application
│       └── ISO8583Demo.csproj   # Project file
├── ISO8583OnDotNet.sln          # Solution file
└── README.md                     # This file
```

## Understanding ISO8583 Messages

### Message Structure

An ISO8583 message consists of:
1. **Message Type Identifier (MTI)**: 4-digit code indicating message type
2. **Bitmap**: Indicates which data elements are present
3. **Data Elements**: Transaction data (card number, amount, etc.)

### Common Message Types

- **0200**: Authorization Request
- **0210**: Authorization Response
- **0400**: Reversal Request
- **0420**: Reversal Response
- **0800**: Network Management Request
- **0810**: Network Management Response

### Key Data Elements

| Field | Description | Example |
|-------|-------------|---------|
| 2 | Primary Account Number (PAN) | 4532015112830366 |
| 3 | Processing Code | 000000 (Purchase) |
| 4 | Transaction Amount | 000000012345 ($123.45) |
| 11 | System Trace Audit Number | 123456 |
| 12 | Local Transaction Time | HHmmss |
| 13 | Local Transaction Date | MMdd |
| 37 | Retrieval Reference Number | 123456789012 |
| 39 | Response Code | 00 (Approved) |
| 41 | Terminal ID | TERM0001 |
| 42 | Merchant ID | MERCHANT000001 |
| 49 | Currency Code | 840 (USD) |

## Usage Example

```csharp
// Create an authorization request
var message = new Iso8583Message
{
    MessageType = "0200"
};

message[2] = "4532015112830366";  // Card number
message[3] = "000000";             // Processing code (Purchase)
message[4] = "000000012345";       // Amount in cents ($123.45)
message[11] = "123456";            // Trace number
message[12] = DateTime.Now.ToString("HHmmss");
message[13] = DateTime.Now.ToString("MMdd");
message[41] = "TERM0001";          // Terminal ID
message[42] = "MERCHANT000001";    // Merchant ID
message[49] = "840";               // Currency (USD)

// Convert to bytes for transmission
byte[] messageBytes = message.ToByteArray();
```

## Testing with Real Endpoints

### Public Test APIs

**Important Note**: Public test endpoints for ISO8583 are extremely rare due to:
- Security and compliance requirements (PCI DSS)
- Financial industry regulations
- Risk of fraud and misuse

### How to Test

To test with real ISO8583 endpoints:

1. **Contact Your Payment Processor**: Reach out to your acquiring bank or payment processor
2. **Get Test Credentials**: They will provide:
   - Test host address and port
   - Terminal and Merchant IDs
   - Test card numbers
   - Message format specifications
3. **Implement TCP/IP Communication**: Add socket communication to send/receive messages
4. **Follow Specifications**: Each processor has specific field requirements

### Common Payment Processors

- Visa DPS (Developer Platform)
- Mastercard Developer Portal
- First Data
- TSYS
- Global Payments
- Elavon

**Note**: Most require business relationships and NDAs before providing test access.

## Next Steps

### Implementing Full Integration

1. **Add TCP/IP Socket Communication**
   ```csharp
   // Example socket connection
   using var client = new TcpClient("test-host.com", 5000);
   using var stream = client.GetStream();
   
   // Send message
   await stream.WriteAsync(messageBytes);
   
   // Receive response
   byte[] response = new byte[1024];
   int bytesRead = await stream.ReadAsync(response);
   ```

2. **Implement Message Parsing**
   - Parse incoming bitmap
   - Extract data elements
   - Validate message integrity

3. **Add Security Features**
   - Message Authentication Codes (MAC)
   - PIN encryption
   - SSL/TLS for transmission

4. **Error Handling**
   - Network timeouts
   - Invalid message formats
   - Response code handling

5. **Logging and Monitoring**
   - Transaction logging
   - Performance monitoring
   - Compliance reporting

## Resources

- [ISO 8583 Wikipedia](https://en.wikipedia.org/wiki/ISO_8583)
- [Payment Card Industry Data Security Standard (PCI DSS)](https://www.pcisecuritystandards.org/)
- Visa Developer Center
- Mastercard Developer Portal

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is for demonstration purposes. Please ensure compliance with all applicable financial regulations and security standards when implementing in production.

## Disclaimer

This is a demonstration project and should NOT be used in production without proper security audits, compliance verification, and integration with certified payment processors. Always follow PCI DSS and other applicable standards when handling payment card data.
