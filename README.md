# Simple CQRS

This is a sample application implemented using a CQRS event sourcing architecture.

This project is inspired by [Greg Young's simple CQRS example](https://github.com/gregoryyoung/m-r) and is meant to be a showcase of the pattern envisioned by Greg Young implemented with modern .NET technologies. If you want a refresh of the theory behind the event sourcing architecture take a look at [this document](https://cqrs.files.wordpress.com/2010/11/cqrs_documents.pdf).

## Purpose and limitations of this project

This project is not meant to be a production ready application. It's purpose in life is to showcase a possible implementation of Greg Young's event sourcing architecture by using modern .NET technologies. You should think of it just as a learning tool: something which is good to study, but not ready for production.

## Libraries used in this project

 - [NEventStore](https://github.com/NEventStore/NEventStore): we used this library in order to implement the `IEventStore` interface. This is one of the most famous .NET based event store implementations. There are alternatives, but we opted for this one because is simple to use and minimalistic in nature. It offers a storage agnostic abstraction over the actual event store and some implementations for concrete persistence layers. In this project we use the [MongoDB implementation](https://github.com/NEventStore/NEventStore.Persistence.MongoDB) 
