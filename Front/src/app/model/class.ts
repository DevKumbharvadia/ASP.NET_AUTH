export class Todo {
    id: string;              // Keep as string since GUIDs are usually represented as strings in TypeScript
    title: string;          // Title of the todo item
    description: string;    // Description of the todo item
    isCompleted: boolean;    // Indicates if the item is completed
    dueDate: Date | null;   // Use Date type for proper date handling, nullable
    createdAt: string ;        // Use Date type for created timestamp
    updatedAt: string;        // Use Date type for updated timestamp
    userId: string | null;  // Keep as string, nullable for foreign key

    constructor() {
        this.id = '';
        this.title = '';
        this.description = '';
        this.isCompleted = false;
        this.dueDate = null; // Start as null for optional date
        this.createdAt = ''; // Set default to current date
        this.updatedAt = ''; // Set default to current date
        this.userId = null; // Start as null
    }
}
