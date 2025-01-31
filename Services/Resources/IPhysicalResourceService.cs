﻿using OwlReadingRoom.Models;
using OwlReadingRoom.Proxy;
using OwlReadingRoom.ViewModels;

namespace OwlReadingRoom.Services.Resources;

public interface IPhysicalResourceService
{
    /// <summary>
    /// Fetches a list of rooms with their desk availability information.
    /// </summary>
    /// <returns>
    /// A list of RoomListViewModel objects containing room details and desk availability.
    /// </returns>
    /// <remarks>
    /// This method performs the following steps:
    /// 1. Retrieves room information and desk counts using a join operation.
    /// 2. Fetches the count of unavailable desks for each room.
    /// 3. Calculates the number of available desks for each room.
    /// 4. Constructs and returns a list of RoomListViewModel objects with the compiled information.
    /// </remarks>
    List<RoomListViewModel> fetchRooms();

    /// <summary>
    /// Retrieves a list of desk information view models for a specific room.
    /// </summary>
    /// <param name="roomId">The ID of the room for which to retrieve desk information.</param>
    /// <returns>
    /// A list of <see cref="DeskInfoViewModel"/> objects containing information about each desk 
    /// in the specified room, including its status, color coding, and reservation details (if any).
    /// </returns>
    /// <remarks>
    /// The method first retrieves the list of desks for the specified room and then fetches the 
    /// corresponding booking information. It iterates through each desk and creates a view model 
    /// that includes the desk's status and reservation message, using the first reservation if 
    /// multiple reservations exist. If no reservation exists, default values are used.
    /// </remarks>
    List<DeskInfoViewModel> GetDeskInfoPerRoom(int roomId);

    /// <summary>
    /// Updates a room's information and adds new desks if specified.
    /// </summary>
    /// <param name="roomViewModel">The view model containing the room's current information.</param>
    /// <param name="roomName">The new name for the room.</param>
    /// <remarks>
    /// This method performs the following operations within a transaction:
    /// 1. Adds new desks to the room if numberOfDesks is greater than 0.
    /// 2. Updates the room's name if it has changed.
    /// If an exception occurs during the process, the transaction is rolled back.
    /// </remarks>
    void UpdateRoom(RoomListViewModel roomViewModel, string roomName);

    /// <summary>
    /// Creates rooms of certain type in the system
    /// </summary>
    /// <param name="roomType">The type of room: AC or Non Ac</param>
    /// <param name="numberOfRooms">The number of rooms</param>
    /// <param name="roomInitials">The initials of room.</param>
    void AddRooms(RoomType roomType, int? numberOfRooms, string roomInitials = "RM");

    /// <summary>
    /// Fetches room list view model by room id.
    /// </summary>
    /// <param name="id">The unique identifier of room.</param>
    /// <returns>The room list view model with selected room details</returns>
    RoomListViewModel FetchRoomById(int id);

    /// <summary>
    /// Fetches room list view model by room type.
    /// </summary>
    /// <param name="roomType">The type of room to be queried.</param>
    /// <returns>The room list view model with selected room details</returns>
    List<RoomListViewModel> FetchRoomsByType(RoomType roomType);
}
