using System;
using System.Text;

namespace ISO8583Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ISO8583 Message Demo ===\n");
            Console.WriteLine("This demo creates sample ISO8583 messages for financial transactions.");
            Console.WriteLine();

            // Demo 1: Authorization Request (0200)
            Console.WriteLine("1. Creating Authorization Request (0200)");
            Console.WriteLine(new string('=', 80));
            CreateAuthorizationRequest();
            
            Console.WriteLine("\n" + new string('-', 80) + "\n");

            // Demo 2: Authorization Response (0210)
            Console.WriteLine("2. Creating Authorization Response (0210)");
            Console.WriteLine(new string('=', 80));
            CreateAuthorizationResponse();
            
            Console.WriteLine("\n" + new string('-', 80) + "\n");

            // Demo 3: Reversal Request (0400)
            Console.WriteLine("3. Creating Reversal Request (0400)");
            Console.WriteLine(new string('=', 80));
            CreateReversalRequest();
            
            Console.WriteLine("\n" + new string('-', 80) + "\n");

            // Demo 4: Network Management (0800)
            Console.WriteLine("4. Creating Network Management Message (0800)");
            Console.WriteLine(new string('=', 80));
            CreateNetworkManagementRequest();

            Console.WriteLine("\n" + new string('=', 80));
            Console.WriteLine("=== Demo Complete ===");
            Console.WriteLine();
            Console.WriteLine("About ISO8583 Testing:");
            Console.WriteLine("  • ISO8583 is the international standard for financial transaction messages");
            Console.WriteLine("  • Used by card networks (Visa, Mastercard, etc.) and payment processors");
            Console.WriteLine("  • Messages are typically sent over TCP/IP connections");
            Console.WriteLine();
            Console.WriteLine("Testing with Real Endpoints:");
            Console.WriteLine("  • Public test endpoints are rare due to security and compliance requirements");
            Console.WriteLine("  • Contact your payment processor for test environment credentials");
            Console.WriteLine("  • Test environments typically require:");
            Console.WriteLine("    - Host address and port");
            Console.WriteLine("    - Terminal/Merchant IDs");
            Console.WriteLine("    - Test card numbers");
            Console.WriteLine("    - Proper message formatting and field specifications");
            Console.WriteLine();
            Console.WriteLine("Next Steps:");
            Console.WriteLine("  1. Get credentials from your payment processor");
            Console.WriteLine("  2. Implement TCP/IP socket communication");
            Console.WriteLine("  3. Add proper error handling and message parsing");
            Console.WriteLine("  4. Implement field specifications per processor requirements");
        }

        static void CreateAuthorizationRequest()
        {
            try
            {
                var message = new Iso8583Message
                {
                    MessageType = "0200" // Authorization Request
                };

                // Primary Account Number (PAN)
                message[2] = "4532015112830366";
                
                // Processing Code (000000 = Purchase)
                message[3] = "000000";
                
                // Transaction Amount (in cents) - $123.45
                message[4] = "000000012345";
                
                // System Trace Audit Number
                message[11] = "123456";
                
                // Local Transaction Time (HHMMSS)
                message[12] = DateTime.Now.ToString("HHmmss");
                
                // Local Transaction Date (MMDD)
                message[13] = DateTime.Now.ToString("MMdd");
                
                // Card Expiry Date (YYMM) - December 2025
                message[14] = "2512";
                
                // Point of Service Entry Mode - Chip card
                message[22] = "051";
                
                // Card Sequence Number
                message[23] = "001";
                
                // Merchant Type/Category Code - Miscellaneous retail
                message[18] = "5999";
                
                // Acquiring Institution ID
                message[32] = "123456";
                
                // Retrieval Reference Number
                message[37] = "123456789012";
                
                // Terminal ID
                message[41] = "TERM0001";
                
                // Merchant ID
                message[42] = "MERCHANT000001";
                
                // Currency Code - USD
                message[49] = "840";

                byte[] messageBytes = message.ToByteArray();
                
                Console.WriteLine("Authorization Request Details:");
                Console.WriteLine($"  Message Type: {message.MessageType}");
                Console.WriteLine($"  Card Number: {message[2]}");
                Console.WriteLine($"  Amount: ${Convert.ToInt64(message[4]) / 100.0:F2} ({message[4]} cents)");
                Console.WriteLine($"  Processing Code: {message[3]} (Purchase)");
                Console.WriteLine($"  Terminal ID: {message[41]}");
                Console.WriteLine($"  Merchant ID: {message[42]}");
                Console.WriteLine($"  Transaction Date/Time: {message[13]}/{message[12]}");
                Console.WriteLine($"  Currency: {message[49]} (USD)");
                Console.WriteLine();
                Console.WriteLine($"Raw Message (Hex): {BitConverter.ToString(messageBytes).Replace("-", "")}");
                Console.WriteLine($"Message Length: {messageBytes.Length} bytes");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        static void CreateAuthorizationResponse()
        {
            try
            {
                var message = new Iso8583Message
                {
                    MessageType = "0210" // Authorization Response
                };

                // Copy request fields
                message[2] = "4532015112830366";
                message[3] = "000000";
                message[4] = "000000012345";
                message[11] = "123456";
                message[12] = DateTime.Now.ToString("HHmmss");
                message[13] = DateTime.Now.ToString("MMdd");
                message[37] = "123456789012";
                message[41] = "TERM0001";
                message[42] = "MERCHANT000001";
                message[49] = "840";

                // Response specific fields
                message[38] = "AUTH01"; // Authorization ID
                message[39] = "00"; // Response Code (00 = Approved)

                byte[] messageBytes = message.ToByteArray();
                
                Console.WriteLine("Authorization Response Details:");
                Console.WriteLine($"  Message Type: {message.MessageType}");
                Console.WriteLine($"  Response Code: {message[39]} (Approved)");
                Console.WriteLine($"  Authorization ID: {message[38]}");
                Console.WriteLine($"  Retrieval Ref: {message[37]}");
                Console.WriteLine();
                Console.WriteLine("Common Response Codes:");
                Console.WriteLine("  00 - Approved");
                Console.WriteLine("  05 - Do not honor");
                Console.WriteLine("  14 - Invalid card number");
                Console.WriteLine("  51 - Insufficient funds");
                Console.WriteLine("  54 - Expired card");
                Console.WriteLine();
                Console.WriteLine($"Raw Message (Hex): {BitConverter.ToString(messageBytes).Replace("-", "")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        static void CreateReversalRequest()
        {
            try
            {
                var message = new Iso8583Message
                {
                    MessageType = "0400" // Reversal Request
                };

                message[2] = "4532015112830366";
                message[3] = "000000";
                message[4] = "000000012345";
                message[11] = "123457"; // New trace number
                message[12] = DateTime.Now.ToString("HHmmss");
                message[13] = DateTime.Now.ToString("MMdd");
                message[37] = "123456789012";
                message[41] = "TERM0001";
                message[42] = "MERCHANT000001";
                
                // Original data elements - simplified for demo
                var originalTime = DateTime.Now.AddMinutes(-5).ToString("HHmmss");
                message[90] = $"0200123456{originalTime}";

                byte[] messageBytes = message.ToByteArray();
                
                Console.WriteLine("Reversal Request Details:");
                Console.WriteLine($"  Message Type: {message.MessageType}");
                Console.WriteLine($"  Reversing Transaction: {message[37]}");
                Console.WriteLine($"  Original Trace: 123456");
                Console.WriteLine($"  New Trace: {message[11]}");
                Console.WriteLine();
                Console.WriteLine("Purpose: Reverse a previously approved transaction");
                Console.WriteLine("Used when: Transaction needs to be cancelled (e.g., timeout, communication error)");
                Console.WriteLine();
                Console.WriteLine($"Raw Message (Hex): {BitConverter.ToString(messageBytes).Replace("-", "")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        static void CreateNetworkManagementRequest()
        {
            try
            {
                var message = new Iso8583Message
                {
                    MessageType = "0800" // Network Management
                };

                // Network Management Information Code
                message[70] = "001"; // Sign-on
                
                // System Trace Audit Number
                message[11] = "123458";
                
                // Local Transaction Time
                message[12] = DateTime.Now.ToString("HHmmss");
                
                // Local Transaction Date
                message[13] = DateTime.Now.ToString("MMdd");

                byte[] messageBytes = message.ToByteArray();
                
                Console.WriteLine("Network Management Request Details:");
                Console.WriteLine($"  Message Type: {message.MessageType}");
                Console.WriteLine($"  Network Management Code: {message[70]} (Sign-on)");
                Console.WriteLine($"  Trace Number: {message[11]}");
                Console.WriteLine();
                Console.WriteLine("Common Network Management Codes:");
                Console.WriteLine("  001 - Sign-on");
                Console.WriteLine("  002 - Sign-off");
                Console.WriteLine("  201 - Echo test");
                Console.WriteLine("  301 - Key exchange");
                Console.WriteLine();
                Console.WriteLine("Purpose: Manage network connections and system status");
                Console.WriteLine();
                Console.WriteLine($"Raw Message (Hex): {BitConverter.ToString(messageBytes).Replace("-", "")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
