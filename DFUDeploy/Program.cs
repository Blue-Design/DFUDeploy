/*
 * This project was designed to work for the NetDFULib code found on the CodeProject 
 * in an article titled ST Micro-electronics Device Firmware Upgrade (DFU/DfuSe) from C#
 * By Mark McLean (ExpElec), 4 Mar 2013 
 * http://www.codeproject.com/Tips/540963/ST-Micro-electronics-Device-Firmware-Upgrade-DFU-D
 * 
 * and is licensed under the The Code Project Open License (CPOL) 1.02,
 * which can be found here
 * http://www.codeproject.com/info/cpol10.aspx 
 * 
 * Revision History:
 * 12/10/2013 Brien Schultz: Added the /C switch to erase the whole chip.
 * 12/10/2013 Brien Schultz: Added the /X switch to exit DFU mode after deploying the bootloader.
*/
using STDFULib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFUDeploy
{
	class Program
    {
        static FirmwareUpdate firmwareUpdate = new FirmwareUpdate();

		static void Main(string[] args)
		{
            if (args.Length == 0)
            {
                Console.WriteLine("Error processing request.");
                Console.WriteLine("");
                Console.WriteLine("Syntax:");
                Console.WriteLine("DFUDeploy [file] [options]");
                Console.WriteLine("");
                Console.WriteLine("Options:");
                Console.WriteLine("\t/c");
                Console.WriteLine("\tClears all memory.  By default only the bootloader memory is erased.");
                Console.WriteLine("\tWith this option your NETMF runtime and deployment will also be erased.");
                Console.WriteLine("");
                Console.WriteLine("\t/x");
                Console.WriteLine("\tExits DFU mode after updating the bootloader.");
                Console.WriteLine("");
                Console.WriteLine("\t/?");
                Console.WriteLine("\tLists devices memory map.");
                Console.WriteLine("");
                Console.WriteLine("WARNING: This tool will deploy to the first USB device found in DFU mode.");
                Console.WriteLine("");
                return;
            }

            try
            {
                if (!firmwareUpdate.IsDFUDeviceFound())
                {
                    throw new Exception("No devices in DFU mode were found.");
                }

                if (args.Contains("/c") || args.Contains("/C"))
                {
                    firmwareUpdate.OnFirmwareUpdateProgress += new FirmwareUpdateProgressEventHandler(firmwareUpdate_OnFirmwareUpdateProgress);

                    UInt16 VID;
                    UInt16 PID;
                    UInt16 Version;

                    try
                    {
                        firmwareUpdate.ParseDFU_File(args[0], out VID, out PID, out Version);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error parsing DFU file. " + ex.Message, ex);
                    }

                    Console.WriteLine("Found VID: " + VID.ToString("X4") + " PID: " + PID.ToString("X4") + " Version: " + Version.ToString("X4"));
                }

                try
                {
                    bool eraseEveything = false;
                    bool exitDFUMode = false;

                    //The cC switch erases everything from the chip
                    if (args.Contains("/c") || args.Contains("/C"))
                    {
                        eraseEveything = true;
                    }

                    //The xX switch exits the device from DFU mode
                    if (args.Contains("/x") || args.Contains("/X"))
                    {
                        exitDFUMode = true;
                    }
                     
                    firmwareUpdate.UpdateFirmware(args[0], eraseEveything, exitDFUMode); 


                }
                catch (Exception ex)
                {
                    throw new Exception("Error deploying DFU file. " + ex.Message, ex);
                }

                Console.WriteLine("Done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
                        
		}


        static void firmwareUpdate_OnFirmwareUpdateProgress(object sender, FirmwareUpdateProgressEventArgs e)
        {
            Console.WriteLine(e.Message + " " + (int)e.Percentage);
        }
	}
}
