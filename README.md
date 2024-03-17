# RevitOutOfContext_gRPC_Console
A minimal CLI server to run remote commands in Revit built on .NET .NET Framework

Key Features • How To Use • Support • License

# Key Features
There are three projects presented here:
RevitOutOfContext_gRPC_Server - Server project ( .NET 7)  

RevitOutOfContext_gRPC_Client - Revit plugin (.NET Framework 4.8) project that checks 
for the presence of an active server and requests commands from it

RevitOutOfContext_gRPC_Protos/RevitOutOfContext_gRPC_ProtosF - Data schema design (google.protobuf) 

# How To Use
To clone and run this application, you'll need Git and Visual Studio installed on your computer.

From your command line:

Clone this repository
$ git clone https://github.com/BIMOpenGroup/RevitOutOfContext_gRPC_Console.git

Open projects in VS

Install dependencies from Nuget

Compile all projects 

In RevitAddinOutOfContext_gRPC_Client.addin change dll path  
Copy Revit addin file RevitAddinOutOfContext_gRPC_Client.addin to your Revit plugins folder 

Run Server from VS

Send commands 

Enjoy :)

# Support
Buy Me A Coffee

# License
MIT
