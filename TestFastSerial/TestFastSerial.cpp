// TestFastSerial.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <tchar.h>
#include <iostream>
#include <chrono>
#include "FastSerial.h"

int main()
{
    namespace FS = FastSerialLib;
    std::string comPort = "COM3";
    int baudrate = 115200;
    int buffersize = 800;
    FS::FastSerial::init(comPort, baudrate, buffersize);
    char output[800];
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

    //test 2 receive EEG data for 10secs after sending p char
    std::chrono::system_clock::time_point timePt;
    timePt = std::chrono::system_clock::now() + std::chrono::seconds(1);
    while (std::chrono::system_clock::now() < timePt);
    FS::FastSerial::writeStringToSerial("p\n");
    timePt = std::chrono::system_clock::now() + std::chrono::seconds(10);
    while (std::chrono::system_clock::now() < timePt) {
        if (FS::FastSerial::available()) {
            len = FS::FastSerial::getString(output);
            for (int i = 0; i < len; i++) {
                std::cout << output[i];
            }
            std::cout << len << std::endl;
        }
    } 
    FS::FastSerial::close();
}