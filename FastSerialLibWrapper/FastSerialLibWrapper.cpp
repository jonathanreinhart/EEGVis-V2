#include "pch.h"
#include "FastSerialLibWrapper.h"

using namespace FastSerialLibWrapper;

void FastSerial::init(String^ t_comPort, int t_baudrate, const int t_bufferSize) {
    std::string t_comPort_std = convertNetStringToCString(t_comPort);
    FastSerialLib::FastSerial::init(t_comPort_std, t_baudrate, t_bufferSize);
}

/// <summary>
/// Writes a String to Serial port
/// </summary>
/// <param name="output"></param>
void FastSerial::writeStringToSerial(String^ output) {
    std::string output_std = convertNetStringToCString(output);
    FastSerialLib::FastSerial::writeStringToSerial(output_std);
}

/// <summary>
/// Prints available Serial data to the console until '\n'
/// </summary>
void FastSerial::coutSerialData() {
    FastSerialLib::FastSerial::coutSerialData();
}

/// <summary>
/// Copies the last line that was sent via Serial to outArray.
/// </summary>
/// <param name="outArray">: char array in which the line of data should be written</param>
/// <returns>length of read string</returns>
String^ FastSerial::getString() {
    char outArray[1000]{};
    FastSerialLib::FastSerial::getString(outArray);
	//convert outArray to String
	String^ managedString = gcnew String(outArray);
    return managedString;
}

/// <summary>
/// Checks if Serial data is available and pushes data to a string for later use
/// </summary>
/// <returns>true if current unread data contains a whole line, else false</returns>
bool FastSerial::available() {
    return FastSerialLib::FastSerial::available();
}

/// <summary>
/// Closes Serial port
/// </summary>
void FastSerial::close() {
    FastSerialLib::FastSerial::close();
}

std::string FastSerial::convertNetStringToCString(String^ managedString){
    msclr::interop::marshal_context context;
    std::string std_string = context.marshal_as<std::string>(managedString);
    return std_string;
}