cmake_minimum_required(VERSION 3.12.4)

include(ExternalProject)
include(GNUInstallDirs)

set(default_build_type "Release")

if(NOT CMAKE_BUILD_TYPE AND NOT CMAKE_CONFIGURATION_TYPES)
  message(STATUS "Set CMAKE_BUILD_TYPE = '${default_build_type}'")
  set(CMAKE_BUILD_TYPE "${default_build_type}" CACHE
      STRING "Choose the type of build." FORCE)

  # Set the possible values of build type for cmake-gui
  set_property(CACHE CMAKE_BUILD_TYPE PROPERTY STRINGS
    "Debug" "Release")
endif()

project(medja 
	VERSION 0.1.0.0 
	DESCRIPTION "medja native api"
)

add_library(medja SHARED 
	native.c
)

# glfw
externalproject_add(
	glfw_download 
	PREFIX glfw
	GIT_REPOSITORY "https://github.com/glfw/glfw.git"
	GIT_TAG 3.3-stable
	GIT_PROGRESS true
	CMAKE_ARGS "-DBUILD_SHARED_LIBS=ON"
		   "-DCMAKE_INSTALL_PREFIX=${CMAKE_BINARY_DIR}"
			 "-DCMAKE_INSTALL_LIBDIR=${CMAKE_INSTALL_LIBDIR}"
		   "-DCMAKE_BUILD_TYPE=${CMAKE_BUILD_TYPE}"
		   "-DGLFW_BUILD_EXAMPLES=OFF"
		   "-DGLFW_BUILD_TESTS=OFF"
		   "-DGLFW_BUILD_DOCS=OFF"
	
	CMAKE_CACHE_ARGS
		"-DCMAKE_C_COMPILER:FILEPATH=${CMAKE_C_COMPILER}"
		"-DCMAKE_CXX_COMPILER:FILEPATH=${CMAKE_CXX_COMPILER}"
)

add_dependencies(medja glfw_download)

set(GLFW_INCLUDE_DIR "${CMAKE_BINARY_DIR}/include")
set(GLFW_LIBRARY_DIR "${CMAKE_BINARY_DIR}/${CMAKE_INSTALL_LIBDIR}")
include_directories(${GLFW_INCLUDE_DIR})

target_link_libraries(medja "${GLFW_LIBRARY_DIR}/libglfw.so.3.3")
