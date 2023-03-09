# Starter repository for Unity + ROS

## Useful commands

Some commands you'll probably use a lot, they're mentioned later in this README as well. 

### Start Docker Container
```sh
docker run -it --rm -p 10000:10000 foxy
```

### Start TCP Endpoint
```sh
ros2 run ros_tcp_endpoint default_server_endpoint --ros-args -p ROS_IP:=0.0.0.0
```

## Getting Started

This comes originally from Unity's examples [link to their github](https://github.com/Unity-Technologies/Unity-Robotics-Hub/tree/main/tutorials/ros_unity_integration). For more in depth info, you should check it out there as well. This repository really just has the fundamental setup for a project.

### Install Docker

This utilizes docker for the ROS containers, which allows you do develop on Windows as well as Linux easily. You will need to install docker on your computer

### Install Unity

This one should be fairly self evident, this uses Unity so you will need Unity...

## Creating Docker Image

I've already included a Dockerfile with the required steps, you can run the following command in the root of the repository:

```sh
docker build -t foxy -f Dockerfile .
```

This one is a one time command, after the initial setup you *shouldn't* need to run again, unless you want to modify the Dockerfile.

## Running the container

You can run the container with the command below. Please note that anything done inside the container after closing it **will be deleted** so make sure to save your code before closing down for the day. Alternatively, you can setup a mounting point that docker can use so that the data is not lost.

```sh
docker run -it --rm -p 10000:10000 foxy
```

## Inside the container

My recommendation would be to do things using tmux, so you can have extra tabs. I included tmux in the Dockerfile so you don't need any extra commands to get it going.

Once you're inside the container, you should run:

```sh
ros2 run ros_tcp_endpoint default_server_endpoint --ros-args -p ROS_IP:=0.0.0.0
```

Once you're here, you're good to go! Everything else can be done like normal ROS, checkout the linked repo or the example publisher and subscriber in the example scripts folder in Unity. All your planners and such can be done in regular Python or C++, whichever you prefer, as it is just normal ROS at that point.