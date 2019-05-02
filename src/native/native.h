#ifndef MEDJA_NATIVE_H
#define MEDJA_NATIVE_H

typedef struct
{
    float x_scale;
    float y_scale;
} medja_screen;

typedef struct
{
    int screen_count;
    medja_screen* screens;  
} medja_system_info;


// this method is just there to have a method that actually does nothing
// but .net will be able to call it, so we can test if the lib is available 
// on the current system
void medja_native_check();

int medja_get_system_info(medja_system_info *system_info);

medja_screen* medja_float();

#endif
