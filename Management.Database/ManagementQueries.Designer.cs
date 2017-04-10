﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Management.Database {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ManagementQueries {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ManagementQueries() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Management.Database.ManagementQueries", typeof(ManagementQueries).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM Users_Lockout WHERE UserId = @UserId.
        /// </summary>
        internal static string LockoutGetByUserId {
            get {
                return ResourceManager.GetString("LockoutGetByUserId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO Users_Lockout (UserId, Attempts) VALUES (@UserId, @Attempts).
        /// </summary>
        internal static string LockoutInsertAttempts {
            get {
                return ResourceManager.GetString("LockoutInsertAttempts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO Users_Lockout (UserId, LockEnd) VALUES (@UserId, @LockEnd).
        /// </summary>
        internal static string LockoutInsertTime {
            get {
                return ResourceManager.GetString("LockoutInsertTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE Users_Lockout SET Attempts = @Attempts WHERE UserId = @UserId.
        /// </summary>
        internal static string LockoutUpdateAttempts {
            get {
                return ResourceManager.GetString("LockoutUpdateAttempts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE Users_Lockout SET LockEnd = @LockEnd WHERE UserId = @UserId.
        /// </summary>
        internal static string LockoutUpdateTime {
            get {
                return ResourceManager.GetString("LockoutUpdateTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO RefreshTokens (UserId, Token, Ticket, Issued, Expires) VALUES (@UserId, @Token, @Ticket, @Issued, @Expires).
        /// </summary>
        internal static string RefreshTokenCreate {
            get {
                return ResourceManager.GetString("RefreshTokenCreate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM RefreshTokens WHERE Token = @Token.
        /// </summary>
        internal static string RefreshTokenDelete {
            get {
                return ResourceManager.GetString("RefreshTokenDelete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM RefreshTokens WHERE Token = @Token.
        /// </summary>
        internal static string RefreshTokenSelect {
            get {
                return ResourceManager.GetString("RefreshTokenSelect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM RefreshTokens WHERE UserId = @UserId.
        /// </summary>
        internal static string RefreshTokenSelectForUser {
            get {
                return ResourceManager.GetString("RefreshTokenSelectForUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE RefreshTokens SET Token = @Token, Ticket = @Ticket, Issued = @Issued, Expires = @Expires WHERE UserId = @UserId.
        /// </summary>
        internal static string RefreshTokenUpdate {
            get {
                return ResourceManager.GetString("RefreshTokenUpdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO Users (Email, PasswordHash) VALUES (@Email, @PasswordHash).
        /// </summary>
        internal static string UserCreate {
            get {
                return ResourceManager.GetString("UserCreate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM Users WHERE Id = @Id.
        /// </summary>
        internal static string UserDeleteById {
            get {
                return ResourceManager.GetString("UserDeleteById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM Users.
        /// </summary>
        internal static string UserGetAll {
            get {
                return ResourceManager.GetString("UserGetAll", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM Users WHERE Email LIKE @Email.
        /// </summary>
        internal static string UserGetByEmail {
            get {
                return ResourceManager.GetString("UserGetByEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM Users WHERE Id = @Id.
        /// </summary>
        internal static string UserGetById {
            get {
                return ResourceManager.GetString("UserGetById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE Users SET Email = @Email, PasswordHash = @PasswordHash, EncryptResults = @EncryptResults, BlockSpeed = @BlockSpeed WHERE Id = @Id.
        /// </summary>
        internal static string UserUpdate {
            get {
                return ResourceManager.GetString("UserUpdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO Votes (CreatedBy, Name, Published, ExpiryDate, ChainString, Questions, EncryptResults, BlockSpeed) VALUES (@CreatedBy, @Name, @Published, @ExpiryDate, @ChainString, @Questions, @EncryptResults, @BlockSpeed).
        /// </summary>
        internal static string VoteCreate {
            get {
                return ResourceManager.GetString("VoteCreate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM Votes WHERE Id=@Id.
        /// </summary>
        internal static string VoteDelete {
            get {
                return ResourceManager.GetString("VoteDelete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM Votes WHERE Id = @Id.
        /// </summary>
        internal static string VoteGetById {
            get {
                return ResourceManager.GetString("VoteGetById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM Votes ORDER BY CreationDate DESC.
        /// </summary>
        internal static string VotesGetAll {
            get {
                return ResourceManager.GetString("VotesGetAll", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM Votes WHERE Published = @Published ORDER BY CreationDate DESC.
        /// </summary>
        internal static string VotesGetByPublished {
            get {
                return ResourceManager.GetString("VotesGetByPublished", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE Votes SET Name=@Name, ChainString=@ChainString, Published=@Published, ExpiryDate=@ExpiryDate, Questions=@Questions WHERE Id=@Id.
        /// </summary>
        internal static string VoteUpdate {
            get {
                return ResourceManager.GetString("VoteUpdate", resourceCulture);
            }
        }
    }
}
