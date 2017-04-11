// Encoding.h

#pragma once


#include <msclr/marshal.h>
using namespace System;
#include "Encoder.h"
#include "Decoder.h"

namespace Encoding1 {

	public ref class Encoding
	{
	public:
		Encoding(int width, int height);
		~Encoding();

		array<unsigned char, 1> ^ GetEncoding(array<unsigned char, 1> ^screen);
		array<unsigned char, 1> ^ GetDecoding(array<unsigned char, 1> ^packet);



	private:
		Encoder *encoder;
		Decoder *decoder;
	};
}
