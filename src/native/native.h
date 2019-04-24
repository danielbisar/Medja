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


int medja_get_system_info(medja_system_info *system_info);

#endif
