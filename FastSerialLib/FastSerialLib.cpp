#include "FastSerialLib.h"

namespace FastSerialLib {
    //initialize class variables
    HANDLE FastSerial::serialHandle;
    DWORD FastSerial::dwBytesRead;
    int FastSerial::bufferSize;
    char* FastSerial::dataReadBuffer;
    std::string FastSerial::curString;
    int FastSerial::available_lines;
    /// <summary>
    /// Opens Serial port on comPort with baudrate of baudrate
    /// </summary>
    /// <param name="comPort">: port to open</param>
    /// <param name="baudrate">: baudrate to use</param>
    /// <param name="buffersize">: size of the data buffer</param>
    /// 
    void FastSerial::init(std::string t_comPort, int t_baudrate, const int t_bufferSize) {
        //init variables
        dwBytesRead = 0;
        available_lines = 0;
        
        //init data buffer
        dataReadBuffer = new char[t_bufferSize];
        bufferSize = t_bufferSize;

        //basic Serial setup
        std::wstring comPortP_temp = std::wstring(t_comPort.begin(), t_comPort.end());
        LPCWSTR comPortP = comPortP_temp.c_str();
        serialHandle = CreateFile(comPortP, GENERIC_READ | GENERIC_WRITE, 0, 0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);
        DCB serialParams = { 0 };
        serialParams.DCBlength = sizeof(serialParams);

        GetCommState(serialHandle, &serialParams);
        serialParams.BaudRate = t_baudrate;
        SetCommState(serialHandle, &serialParams);

        // Set timeouts
        COMMTIMEOUTS timeout = { 0 };
        timeout.ReadIntervalTimeout = 50;
        timeout.ReadTotalTimeoutConstant = 50;
        timeout.ReadTotalTimeoutMultiplier = 50;
        timeout.WriteTotalTimeoutConstant = 50;
        timeout.WriteTotalTimeoutMultiplier = 10;

        SetCommTimeouts(serialHandle, &timeout);
    }
	
    /// <summary>
    /// write String to Serial
    /// </summary>
    /// <param name="output">output string</param>
    /// <returns>true if successful</returns>
    bool FastSerial::writeStringToSerial(std::string output) {
        char* outputArray = &output[0];
        if (!WriteFile(serialHandle, outputArray, output.length(), &dwBytesRead, NULL)) {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Prints available Serial data to the console until '\n'
    /// </summary>
    void FastSerial::coutSerialData() {
        bool got_end = false;
        while (!got_end) {
            while (!ReadFile(serialHandle, dataReadBuffer, bufferSize, &dwBytesRead, NULL));
            for (int i = 0; i < dwBytesRead; i++) {
                char curData = dataReadBuffer[i];
                std::cout << curData;
                if (curData == '\n')
                    got_end = true;
            }
        }
    }

    /// <summary>
    /// Copies the last line that was sent via Serial to outArray.
    /// </summary>
    /// <param name="outArray">: char array in which the line of data should be written</param>
    /// <returns>length of read string</returns>
    int FastSerial::getString(char* outArray) {
        int line_end = curString.find('\n');
        //if there exists \n in curString
        if (line_end != -1) {
            //copy the first line to outArray and erase it from curString
            std::string outString = curString;
            outString.erase(line_end + 1);
            strcpy(outArray, outString.c_str());
            curString.erase(0, line_end + 1);
            available_lines--;
            return outString.length();
        }
        return 0;
    }

    /// <summary>
    /// Checks if Serial data is available and pushes data to a string for later use
    /// </summary>
    /// <returns>true if current unread data contains a whole line, else false</returns>
    bool FastSerial::available() {
        if (ReadFile(serialHandle, dataReadBuffer, bufferSize, &dwBytesRead, NULL)) {
            for (int i = 0; i < dwBytesRead; i++) {
                char curData = dataReadBuffer[i];
                curString += curData;
                if (curData == '\n')
                    available_lines++;
            }
            if (available_lines > 0)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Closes Serial port
    /// </summary>
    void FastSerial::close() {
        CloseHandle(serialHandle);
    }
}