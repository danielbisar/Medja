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

int runClient()
{
    printf ("Connecting to hello world server…\n");
    void *context = zmq_ctx_new ();
    void *requester = zmq_socket (context, ZMQ_REQ);
    zmq_connect (requester, "tcp://localhost:5555");

    int request_nbr;
    for (request_nbr = 0; request_nbr != 10; request_nbr++) {
        char buffer [10];
        printf ("Sending Hello %d…\n", request_nbr);
        zmq_send (requester, "Hello", 5, 0);
        zmq_recv (requester, buffer, 10, 0);
        printf ("Received World %d\n", request_nbr);
    }
    zmq_close (requester);
    zmq_ctx_destroy (context);
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
    bool isServer = true;

    if(argc > 1)
    {
        char* modeFlag;
        modeFlag = argv[1];

        if(strcmp(modeFlag, "-c") == 0)
            isServer = false;
    }

    printf("ZeroMQ Testl\nClient mode: %d (default = false, enable with flag -c)\n", !isServer);
    printZeroMqVersion();

    if(isServer)
        return runServer();
    else
        return runClient();
}
