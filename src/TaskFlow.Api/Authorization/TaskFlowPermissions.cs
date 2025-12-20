namespace TaskFlow.Api.Authorization;

public static class TaskFlowPermissions
{
    public static class Boards
    {
        public const string Update = "boards:update";
        public const string Delete = "boards:delete";
        public const string AddBoardMember = "boards:add-member";
        public const string RemoveBoardMember = "boards:remove-member";
        public const string ChangeBoardMemberRole = "boards:change-member-role";
    }
    
    public static class Columns
    {
        public const string Create = "columns:create";
        public const string Update = "columns:update";
        public const string Delete = "columns:delete";
        public const string Move = "columns:move";
    }
    
    public static class Cards
    {
        public const string Create = "cards:create";
        public const string Update = "cards:update";
        public const string Delete = "cards:delete";
        public const string Assign = "cards:assign";
    }

}