using System;
using LeitourApi.Interfaces;
using LeitourApi.Models;
using LeitourApi.Data;

namespace LeitourApi.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private LeitourContext _context;
        public UserRepository? userRepository {get;}
        private PostRepository? postRepository {get;}
        private CommentRepository?  commentRepository {get;}
        private Repository<Annotation>? annotationRepository {get;}
        private SavedBookRepository? savedRepository {get;}
        public UnitOfWork(LeitourContext context) => _context = context;
        
        public void Commit() => _context.SaveChanges();
        public IUserRepository UserRepository => userRepository ?? new UserRepository(_context);
        public IPostRepository PostRepository => postRepository ?? new PostRepository(_context);
        public IRepository<Annotation> AnnotationRepository => annotationRepository ?? new Repository<Annotation>(_context);
        public ISavedBookRepository SavedRepository => savedRepository ?? new SavedBookRepository(_context);
        public IRepository<Comment> CommentRepository => commentRepository ?? new Repository<Comment>(_context);

       private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    _context.Dispose();
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
}
}