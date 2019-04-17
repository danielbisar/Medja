cmake_minimum_required(VERSION 3.14.2)

project(medja 
	VERSION 0.1.0.0 
	DESCRIPTION "medja native api"
)

#set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR})
#set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR})
#set_target_properties(medja PROPERTIES VERSION ${PROJECT_VERSION}) # medja.so.1.0.0 or alike
#set_target_properties(medja PROPERTIES SOVERSION 1)		   # create symlink medja.so.1

# glfw
include(ExternalProject)

externalproject_add(
	glfw_download 
	PREFIX glfw
	GIT_REPOSITORY "https://github.com/glfw/glfw.git"
	GIT_TAG 3.3-stable
	GIT_PROGRESS true
	CMAKE_ARGS "-DBUILD_SHARED_LIBS=ON"
		   "-DCMAKE_INSTALL_PREFIX=${CMAKE_BINARY_DIR}/libs"
		   "-DCMAKE_BUILD_TYPE=Release"
		   "-DGLFW_BUILD_EXAMPLES=OFF"
		   "-DGLFW_BUILD_TESTS=OFF"
		   "-DGLFW_BUILD_DOCS=OFF"
	
	CMAKE_CACHE_ARGS
		"-DCMAKE_C_COMPILER:FILEPATH=${CMAKE_C_COMPILER}"
		"-DCMAKE_CXX_COMPILER:FILEPATH=${CMAKE_CXX_COMPILER}"
)

set(GLFW_INCLUDE_DIR "${CMAKE_BINARY_DIR}/libs")
set(GLFW_LIB_DIR "${CMAKE_BINARY_DIR}/libs/libs64")

add_library(glfw SHARED IMPORTED)
set_target_properties(glfw
	PROPERTIES IMPORTED_LOCATION ${GLFW_LIB_DIR}/libglfw.so
)



include_directories(${GLFW_INCLUDE_DIR})
add_library(medja SHARED 
	native.c
)
target_link_libraries(glfw)
add_dependencies(medja glfw_download)


