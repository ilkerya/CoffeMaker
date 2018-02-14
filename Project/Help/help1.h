
Flash500ByteCountPrev
                       Flash500ByteCount++;

                       int     Flash500ByteWriteTimer = 0;// timeout i sifirla
                      int      Flash500ByteWriteTimeoutTimer = 0; // timeout




        public static int[] Var_CommandType = new int[30]; // command tipi
        public static int[] Var_Command = new int[30]; // command tipi
        public static int[] Var_Parameter1 = new int[30]; // parameter  sayisi
        public static int[] Var_Parameter2 = new int[30]; // parameter sayisi
        public static int[] Var_Parameter3 = new int[30]; // parameter sayisi
        public static int[] Var_Parameter4 = new int[30]; // parameter sayisi



           public void DATA_populate_CommandArrays_From_NumericBoxesLine00(uint i)
        {
            Okyanus.Variables.Var_Command[i] = (UInt16)numericUpDown_Par0.Value;
            Okyanus.Variables.Var_Parameter1[i] = (UInt16)numericUpDown_Par1.Value;
            Okyanus.Variables.Var_Parameter2[i] = (UInt16)numericUpDown_Par2.Value;
            Okyanus.Variables.Var_Parameter3[i] = (UInt16)numericUpDown_Par3.Value;
            Okyanus.Variables.Var_Parameter4[i] = (UInt16)numericUpDown_Par4.Value;
        }


DATA_populate_CommandArrays_From_NumericBoxesLine00(0);



             for (i = 0; i < 10; i++)
            {
                // data
                SP1_Buffer[j] = (byte)(Okyanus.Variables.Var_Command[i] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 1] = (byte)(Okyanus.Variables.Var_Command[i]);
                SP1_Buffer[j + 2] = (byte)(Okyanus.Variables.Var_Parameter1[i] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 3] = (byte)(Okyanus.Variables.Var_Parameter1[i]);
                SP1_Buffer[j + 4] = (byte)(Okyanus.Variables.Var_Parameter2[i] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 5] = (byte)(Okyanus.Variables.Var_Parameter2[i]);
                SP1_Buffer[j + 6] = (byte)(Okyanus.Variables.Var_Parameter3[i] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 7] = (byte)(Okyanus.Variables.Var_Parameter3[i]);
                SP1_Buffer[j + 8] = (byte)(Okyanus.Variables.Var_Parameter4[i] >> SHIFT8); // firmware komut
                SP1_Buffer[j + 9] = (byte)(Okyanus.Variables.Var_Parameter4[i]);
                j += 10;

            }

   SP1_Buffer[i] ->
   SP1_SendBuf[i] ->
   SP1_serialPort -> RS232
