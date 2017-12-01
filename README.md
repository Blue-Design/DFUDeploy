# DFUDeploy
Tool for deploying netMF binaries to STM chips using either the Windows Classic Desktop or Universal Windows platforms.  

Below is example syntax for using this library...

```
if (!firmwareUpdate.IsDFUDeviceFound())
{
    throw new Exception("No devices in DFU mode were found.");
}

if (args.Contains("/c") || args.Contains("/C"))
{
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

    Debug.WriteLine("Found VID: " + VID.ToString("X4") + " PID: " + PID.ToString("X4") + " Version: " + Version.ToString("X4"));
}

try
{
    bool eraseEveything = false;
    bool exitDFUMode = false;
    firmwareUpdate.UpdateFirmware(args[0], eraseEveything, exitDFUMode);  
}
catch (Exception ex)
{
    throw new Exception("Error deploying DFU file. " + ex.Message, ex);
}

Debug.WriteLine("Done.");
```
