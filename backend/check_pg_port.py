import socket
import time

TIMEOUT=300

def check_port(host, port):
    try:
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        sock.settimeout(1)
        sock.connect((host, port))
        sock.shutdown(socket.SHUT_RDWR)
        return True
    except:
        return False

if __name__ == '__main__':
    host_up = False
    while TIMEOUT > 0 and host_up is False:
        host_up = check_port('db.chat.bg', 5432)
        TIMEOUT -= 1
