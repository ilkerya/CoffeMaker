using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Okyanus
{
     class SP1
    {
         public   static  uint ComTimeout=0;  // SP1.ComTimeout
         public static uint ErrorCount = 0;
     //   uint ReadSequence;
         public static uint ReadSequence = 0;
         public static uint Preamble = 0;
        public static uint Length = 88;
        public static uint Data1 = 0;
        public static uint Data2 = 0;
        public static uint Data3 = 0;
        public static uint Data4 = 0;
        public static uint Data5 = 0;
        public static uint CRC = 0;

        public static uint RecCommand = 0;
        public static uint RecAdress = 0;
        public static uint RecAdrLength = 0;

        public static byte[] ReceiveBuf = new byte[256]; // preamble+length+crc + 4*4 byte data  4 byte + 4 byte + 4 byte + 16 byte
        public static byte[] SendBuf = new byte[256]; // preamble+length+crc + 4*4 byte data  4 byte + 4 byte + 4 byte + 16 byte

        public static byte[] Buffer = new byte[256];

        public static UInt16 Send_TotLength = 0;
        public static uint SendCommand = 0;
        public static uint SendAdress = 0;
        public static uint Send_AdrLength = 0;
        public static uint SendData1 = 234;
        public static uint SendData2 = 567;
        public static uint SendData3 = 34567;
        public static uint SendData4 = 11134;
        public static uint SendData5 = 11134;
        public static uint SendCRC = 0;

        public static uint CRC_Calc = 0;
        public static uint CRC_Error = 0;

        public static int ReadTimeout = 0;

     //    for(int i = 0; int< 3;int++){

  //   }


    }
     class SP2
     {
         public static uint ComTimeout = 0;  // SP1.ComTimeout
         public static uint ErrorCount = 0;
         //   uint ReadSequence;
         public static uint ReadSequence = 0;
         public static uint Preamble = 0;
         public static uint Length = 88;
         public static uint Data1 = 0;
         public static uint Data2 = 0;
         public static uint Data3 = 0;
         public static uint Data4 = 0;
         public static uint Data5 = 0;
         public static uint CRC = 0;

         public static uint RecCommand = 0;
         public static uint RecAdress = 0;
         public static uint RecAdrLength = 0;

         public static byte[] ReceiveBuf = new byte[256]; // preamble+length+crc + 4*4 byte data  4 byte + 4 byte + 4 byte + 16 byte
         public static byte[] SendBuf = new byte[256]; // preamble+length+crc + 4*4 byte data  4 byte + 4 byte + 4 byte + 16 byte

         public static byte[] Buffer = new byte[256];

         public static UInt16 Send_TotLength = 0;
         public static uint SendCommand = 0;
         public static uint SendAdress = 0;
         public static uint Send_AdrLength = 0;
         public static uint SendData1 = 234;
         public static uint SendData2 = 567;
         public static uint SendData3 = 34567;
         public static uint SendData4 = 11134;
         public static uint SendData5 = 11134;
         public static uint SendCRC=0;

         public static uint CRC_Calc = 0;
         public static uint CRC_Error = 0;
     }
}
