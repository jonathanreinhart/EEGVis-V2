// TestFastSerial.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <tchar.h>
#include <iostream>
#include <chrono>
#include "FastSerial.h"

int main()
{
    namespace FS = TestFastSerial;
    std::string comPort = "COM4";
    int baudrate = 2000000;
    FS::FastSerial::init(comPort, baudrate, 1);
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
        //if (FS::FastSerial::available()) {
        //    char outArray[10000];
        //    FS::FastSerial::getString(outArray);
        //    std::cout /* << outArray << " " */ << i << " " << std::chrono::duration_cast<std::chrono::milliseconds>(std::chrono::system_clock::now() - timePt + std::chrono::seconds(10)).count() << std::endl;
        //    //std::cout << outArray << std::endl;
        //    i++;
        //}
        if (FS::FastSerial::available()) {
            std::vector<uint32_t> outVec;
            outVec = FS::FastSerial::get24Array();
            std::cout /* << outArray << " " */ << i << " " << std::chrono::duration_cast<std::chrono::milliseconds>(std::chrono::system_clock::now() - timePt + std::chrono::seconds(100)).count() << '\n';
            std::cout << outVec.size() << '\n';
            /*for (int l = 0; l < outVec.size(); l++) {
                std::cout << outVec.at(l) << " ";
            }*/
            std::cout << std::endl;
            i++;
        }
    } 
    FS::FastSerial::close();
}