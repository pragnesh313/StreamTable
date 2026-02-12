import { streamService } from './streamService.js';
const { scan, throttleTime } = rxjs.operators;

const initCustomerTable = (selector) => {
    const table = $(selector).DataTable({
        columns: [
            { data: "id" },
            { data: "firstName" },
            { data: "lastName" },
            { data: "contact" },
            { data: "email" },
            {
                data: "dateOfBirth",
                render: (data) => data ? new Date(data).toLocaleDateString() : ''
            },
            {
                render: (data, type, row) =>
                    `<a href='#' class='btn btn-sm btn-primary' onclick="Edit('${row.id}')">Edit</a>`
            },
        ],
        order: [[5, 'desc']],
        deferRender: true // Performance boost for large datasets
    });

    // Subscribing to the separated logic
    streamService.getStream('/api/customer')
    // For sql query
    streamService.getStream('/api/customer/sql')
        .pipe(
            // Accumulate rows as they arrive
            scan((acc, newRows) => [...acc, ...newRows], []),
            // Update UI every 100ms instead of every single chunk to save CPU
            throttleTime(100, rxjs.asyncScheduler, { leading: true, trailing: true })
        )
        .subscribe({
            next: (allRows) => {
                table.clear();
                table.rows.add(allRows);
                table.draw(false);
            },
            error: (err) => console.error("Table Stream Error:", err)
        });
};

// Start the table
$(document).ready(() => initCustomerTable("#customerTable"));