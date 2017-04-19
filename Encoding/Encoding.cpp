// This is the main DLL file.

#include "stdafx.h"

#include "Encoding.h"



VideoEncoding::Encoding::Encoding(int width, int height)
{
	encoder = new Encoder(width, height);
	decoder = new Decoder(width, height);
}

VideoEncoding::Encoding::!Encoding()
{
	delete encoder;
	delete decoder;
}



array<unsigned char, 1>^ VideoEncoding::Encoding::GetEncoding(array<unsigned char, 1>^ screen)
{
	pin_ptr<System::Byte> pToScreen = &screen[0];
	unsigned char* pby = pToScreen;
	char* im = reinterpret_cast<char*>(pby);

	int size = 0;
	char *encodedData = encoder->encode(im, &size);
	
	array<unsigned char, 1> ^data = gcnew array<unsigned char, 1>(size);
	if (encodedData)
	{
		System::Runtime::InteropServices::Marshal::Copy(IntPtr((void *)encodedData), data, 0, size);
	}
	return data;
}

array<unsigned char, 1>^ VideoEncoding::Encoding::GetDecoding(array<unsigned char, 1>^ packet)
{
	pin_ptr<System::Byte> pToPacket = &packet[0];
	unsigned char* pby = pToPacket;
	char* packetString = reinterpret_cast<char*>(pby);
	int imSize = 0;
	char *im = decoder->ReadFrame(packetString, packet->Length, &imSize); //must deallocate im if it isn't NULL;
	array<unsigned char, 1> ^imData = gcnew array<unsigned char, 1>(imSize);
	if (im && imSize)
	{
		System::Runtime::InteropServices::Marshal::Copy(IntPtr((void *)im), imData, 0, imSize);
		delete im;
	}
	return imData;
}

array<unsigned char, 1>^ VideoEncoding::Encoding::CaptureBitmap()
{
	int size = 0;
	char *bitmap = (char *)encoder->getBitmap(&size);
	array<unsigned char, 1> ^bitmapArray = gcnew array<unsigned char, 1>(size);

	if (bitmap)
	{
		System::Runtime::InteropServices::Marshal::Copy(IntPtr((void *)bitmap), bitmapArray, 0, size);
	}

	return bitmapArray;
}
