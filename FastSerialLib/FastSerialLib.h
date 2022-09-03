#pragma once
#pragma warning(disable : 4996)

#include <windows.h>
#include <tchar.h>
#include <chrono>
#include <iostream>
#include <string>

namespace FastSerialLib {
	class FastSerial {
	public:
		__declspec(dllexport) static void init(std::string t_comPort, int t_baudrate, const int t_buffersize);
		__declspec(dllexport) static bool available();
		__declspec(dllexport) static void coutSerialData();
		__declspec(dllexport) static int getString(char* outArray);
		__declspec(dllexport) static void close();
		__declspec(dllexport) static bool writeStringToSerial(std::string output);

	private:
		static HANDLE serialHandle;
		static DWORD dwBytesRead;
		static int bufferSize;
		static char* dataReadBuffer;
		//used to store incoming Serial data for later use
		static std::string curString;
		//counts how many lines curString contains
		static int available_lines;
	};
}