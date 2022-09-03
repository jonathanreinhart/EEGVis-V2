// TestFastSerial.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <tchar.h>
#include <iostream>
#include <chrono>
#include "FastSerial.h"

int main()
{
    namespace FS = FastSerialLib;
    std::string comPort = "COM5";
    int baudrate = 115200;
    FS::FastSerial::init(comPort, baudrate);
    char output[1];
    int len;

    /*
    //test 1: receive and send data
    while (!available());
    len = getString(output);
    for (int i = 0; i < len; i++) {
        cout << output[i];
    }
    writeStringToSerial("hello\n");
    while (!available());
    len = getString(output);
    for (int i = 0; i < len; i++) {
        cout << output[i];
    }
    */

    int i = 0;

    //test 2 receive EEG data for 10secs after sending p char
    std::chrono::system_clock::time_point timePt;
    timePt = std::chrono::system_clock::now() + std::chrono::seconds(2);
    while (std::chrono::system_clock::now() < timePt);
    FS::FastSerial::writeStringToSerial("p\n");
    timePt = std::chrono::system_clock::now() + std::chrono::seconds(1);
    while (std::chrono::system_clock::now() < timePt);
    timePt = std::chrono::system_clock::now() + std::chrono::seconds(10);
    while (std::chrono::system_clock::now() < timePt) {
        if (FS::FastSerial::available()) {
            char outArray[600];
            FS::FastSerial::getString(outArray);
            std::cout /* << outArray */<< i << " " << std::chrono::duration_cast<std::chrono::milliseconds>(std::chrono::system_clock::now() - timePt + std::chrono::seconds(10)).count() << std::endl;
            i++;
        }
    } 
    FS::FastSerial::close();
}