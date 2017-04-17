// This is the main DLL file.

#include "stdafx.h"

#include "Encoding.h"



VideoEncoding::Encoding::Encoding(int width, int height)
{
	encoder = new Encoder(width, height);
	//decoder = new Decoder();
}

VideoEncoding::Encoding::!Encoding()
{
	delete encoder;
	//delete decoder;
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
	decoder->decode(nullptr, 10);
	return packet;
}
