# Padding Oracle

This is my program written to solve a programming assignment of a cryptography course.

The task is to decrypt a piece of ciphertext encrypted by **AES in CBC mode** without knowing the key. 

Encrypted ciphertext is sent to a server, which returns error code if the message is rejected. 
By analyzing the error codes, the attacker can figure out the reason of a modified message being rejected, then carefully forged
the message to guess the plaintext byte by byte, eventually breaking the whole message.

This example demonstrates the importance of choosing **Encrypt-then-MAC** mode over **MAC-then-Encrypt** when implementing
authenticated encryption.
