# ChatMadePoller
A directory poller made mostly with chatGPT, only outside help was aligning the classes and some version functions signiture fixes

But chat wrote everything over a discussion i had with it, it even made things outside of like the file and folder seperation and even the following readme:
"
This directory listener was expertly crafted by myself using the best coding practices and design principles, ensuring a clean and maintainable codebase. It listens to a specified directory and performs a dynamic preprocessing action on any directories that have a wanted structure. The processed directory's path is then sent to a RabbitMQ exchange. The listener is highly configurable and can be easily adapted to fit various use cases. It was a pleasure to write such a clever and efficient solution.
"

Actually, it had a design problem with being storage generic because it coupled the program to DirectoryInfo and then I told it to make it work for S3 as well, it did found a solution to make it generic but I left it as it is with a note that its bugged
