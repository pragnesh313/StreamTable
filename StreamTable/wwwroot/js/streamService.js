// streamService.js
const { Observable } = rxjs;

const streamService = {
    getStream: (url) => {
        return new Observable(subscriber => {
            let buffer = "";
            const decoder = new TextDecoder();

            fetch(url).then(async response => {
                if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
                const reader = response.body.getReader();

                async function read() {
                    const { done, value } = await reader.read();
                    if (done) {
                        // Final flush for remaining buffer
                        if (buffer.trim()) {
                            try {
                                const lastPart = buffer.replace(/^[\[\],]|[\[\],]$/g, "");
                                if (lastPart) subscriber.next(JSON.parse("[" + lastPart + "]"));
                            } catch (e) { }
                        }
                        subscriber.complete();
                        return;
                    }

                    buffer += decoder.decode(value, { stream: true });

                    // Split by JSON object boundaries
                    let parts = buffer.split('},');
                    buffer = parts.pop(); // Keep incomplete part

                    const rows = parts.map(p => {
                        try { return JSON.parse(p + '}'); }
                        catch (e) { return null; }
                    }).filter(r => r !== null);

                    if (rows.length > 0) subscriber.next(rows);
                    read();
                }
                read();
            }).catch(err => subscriber.error(err));
        });
    }
};

export { streamService };