#pragma once
#pragma warning(disable : 4996)

#include <windows.h>
#include <tchar.h>
#include <chrono>
#include <iostream>
#include <string>

#define DLLExportFunc extern "C" __declspec(dllexport)

namespace FastSerial {
	DLLExportFunc void init(std::string t_comPort, int t_baudrate, const int t_buffersize);
	DLLExportFunc bool available();
	DLLExportFunc void coutSerialData();
	DLLExportFunc int getString(char* outArray);
	DLLExportFunc void close();
	DLLExportFunc void writeStringToSerial(std::string output);
}
