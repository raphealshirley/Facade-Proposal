import socket
import sys
import os
import io


# Create communication channel
TCPsock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
UDPsock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

# Bind the socket to the port
TCPserver_address = ('', 8052) #accept from any address
UDPserver_address = ('192.168.2.227', 8881) #address and port of the laptop
print('starting up on {} port {}'.format(*TCPserver_address))
TCPsock.bind(TCPserver_address)
TCPsock.listen(1) # Listen for incoming connections

while True:
    print('waiting for a connection from the mobile device')
    connection, client_address = TCPsock.accept()

    try:
        print('connection from', client_address)

        #create (or overwrite) existing image.png everytime a stream for a new screencapture is coming
        if os.path.isfile("image.png"):
            os.remove("image.png")
        file = open("image.png","w+b")

        # Receive the data
        while True:
            data = connection.recv(1024)
            if data:
                file.write(data) #write every incoming chunk of data to the image.png
                print(len(data)) #for debugging
            else:
                print('no data from', client_address)
                break
    finally:
        connection.close()

    # message = yourDetectionMethod("image.png")
    message = "[[(x1,y1),(x2,y2),(x3,y3),(x4,y4)],[(x1,y1),(x2,y2),(x3,y3),(x4,y4)]]" #an example message
    UDPsock.sendto(message.encode('utf-8'), UDPserver_address) #send message to mobile
    print("message sent")