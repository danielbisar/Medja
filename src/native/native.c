// native.c
// this library provides access to some system functions that were not available via other
// .net projects

#include <GLFW/glfw3.h>
#include <stdio.h>
#include <stdlib.h>
#include "native.h"

void __attribute__ ((constructor)) init_medja() {
	printf("Medja.native: Initialize library\n");
}

void __attribute__ ((destructor)) free_medja() {
	printf("Medja.native: Exit library\n");
}

int medja_get_system_info(medja_system_info *system_info) {
	
    if(!system_info)
        return -1;
    
    if(!glfwInit())
        return -1;

    // monitors is allocated by glfw and freed by glfw
    int count;
    GLFWmonitor** monitors = glfwGetMonitors(&count);

    system_info->screen_count = count;
    system_info->screens = (medja_monitor*)malloc(sizeof(medja_monitor) * count);

    for(int i = 0; i < count; i++)
    {
        GLFWmonitor* monitor = monitors[i];
        float xScale;
        float yScale;

        glfwGetMonitorContentScale(monitor, &xScale, &yScale);

        system_info->screens[i].x_scale = xScale;
        system_info->screens[i].y_scale = yScale;
    }

	glfwTerminate();

    // todo free returned values?

    return 0;
}
