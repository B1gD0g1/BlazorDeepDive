﻿using Microsoft.EntityFrameworkCore;
using ServerManagement.Data;

namespace ServerManagement.Models
{
    public class SeversEFCoreRepository : ISeversEFCoreRepository
    {
        private readonly IDbContextFactory<ServerManagementContext> contextFactory;

        public SeversEFCoreRepository(IDbContextFactory<ServerManagementContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void AddServer(Server server)
        {
            //获取数据库
            using var db = this.contextFactory.CreateDbContext();
            //db是数据库，Servers是表
            db.Servers.Add(server);

            db.SaveChanges();
        }

        public List<Server> GetServers()
        {
            using var db = this.contextFactory.CreateDbContext();
            //ToList方法是从表中运行一个SELECT语句
            return db.Servers.ToList();
        }

        public List<Server> GetServersByCity(string cityName)
        {
            using var db = this.contextFactory.CreateDbContext();
            return db.Servers.
                Where(x => x.City != null
                && x.City.ToLower().IndexOf(cityName.ToLower()) >= 0).ToList();
        }

        public Server? GetServerById(int id)
        {
            using var db = this.contextFactory.CreateDbContext();
            var server = db.Servers.Find(id);

            if (server is not null)
                return server;

            return new Server();
        }

        public void UpdateServer(int serverId, Server server)
        {
            if (server == null) throw new ArgumentNullException(nameof(server));
            if (serverId != server.ServerId) return;

            using var db = this.contextFactory.CreateDbContext();
            var serverToUpdate = db.Servers.Find(serverId);

            if (serverToUpdate is not null)
            {
                serverToUpdate.Name = server.Name;
                serverToUpdate.City = server.City;
                serverToUpdate.IsOnline = server.IsOnline;

                db.SaveChanges();
            }
        }

        public void DeleteServer(int serverId)
        {
            using var db = this.contextFactory.CreateDbContext();
            var server = db.Servers.Find(serverId);

            if (server == null) return;

            db.Servers.Remove(server);
            db.SaveChanges();
        }

        public List<Server> SearchServers(string serverFilter)
        {
            using var db = this.contextFactory.CreateDbContext();
            return db.Servers.Where(x =>
                x.Name != null &&
                x.Name.ToLower().IndexOf(serverFilter.ToLower()) >= 0).ToList();
        }
    }
}
