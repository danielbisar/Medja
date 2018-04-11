cmake_minimum_required(VERSION 3.0)
project(zeromq-vsc)
set(SOURCES main.cpp)

add_executable(${PROJECT_NAME} ${SOURCES})
target_link_libraries(${PROJECT_NAME} zmq)
