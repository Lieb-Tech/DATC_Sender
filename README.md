# DATC_Sender
Distributed AirTrafficControl

Inspired by PiAware's system, I wanted to create a system to collect and process ADS-B data. Given the amount of data they are processing, it stands to reason it's a greatly distributed system.

This application will run on the RaspberryPi to collect the raw data - to do so it makes HTTP calls to the PiAware server which is part of the locally hosted monitoring webpage. 
That data is then serialized and uploaded to an Azure Event Hub (Kafka-like) ingestion service.  The data is then processed by the DATC_Receiver project. 

In an effort to keep the programming pattern the same in both Sender & Receiver, the application uses Akka.Net.

Video of system running -- Receiver on left, Sender on right  
[![](http://img.youtube.com/vi/SL75g1Sdo4A/0.jpg)](http://www.youtube.com/watch?v=SL75g1Sdo4A "DATC running")
