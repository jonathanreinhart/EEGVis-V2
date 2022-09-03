#pragma once

#include <windows.h>
#include <tchar.h>
#include <chrono>
#include <iostream>
#include <string>
#include <msclr\marshal_cppstd.h>

using namespace System;

namespace FastSerialLibWrapper {
	public ref class FastSerial
	{
	public:
		void init(String^ t_comPort, int t_baudrate, const int t_buffersize);
		bool available();
		void coutSerialData();
		String^ getString();
		void close();
		bool writeStringToSerial(String^ output);
	private:
		std::string convertNetStringToCString(String^ output);
	};
}
