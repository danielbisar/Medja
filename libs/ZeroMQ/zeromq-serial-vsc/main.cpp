#include <zmq.h>
#include <stdio.h>
#include <unistd.h>
#include <string.h>
#include <assert.h>

int runServer()
{
    //  Socket to talk to clients
    void *context = zmq_ctx_new ();
    void *responder = zmq_socket (context, ZMQ_REP);
    int rc = zmq_bind (responder, "tcp://*:5555");
    assert (rc == 0);

    while (1) {
        char buffer [10];
        zmq_recv (responder, buffer, 10, 0);
        printf ("Received: %s\n", buffer);
        sleep (1);          //  Do some 'work'
        zmq_send (responder, "World", 5, 0);
    }

    return 0;
}

void printZeroMqVersion()
{
    int major, minor, patch;
    zmq_version (&major, &minor, &patch);
    printf ("Current 0MQ version is %d.%d.%d\n", major, minor, patch);
}

int main (int argc, char** argv)
{
    printf("ZeroMQ Testl\n");
    printZeroMqVersion();

    uint32_t *data;


    return runServer();
}
