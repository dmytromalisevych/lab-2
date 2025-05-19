window.initializeIndexedDB = async (dbName, version) => {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(dbName, version);

        request.onerror = () => reject(request.error);
        request.onsuccess = () => resolve();

        request.onupgradeneeded = (event) => {
            const db = event.target.result;
            
            if (!db.objectStoreNames.contains('users')) {
                db.createObjectStore('users', { keyPath: 'id', autoIncrement: true });
            }
            if (!db.objectStoreNames.contains('doctors')) {
                db.createObjectStore('doctors', { keyPath: 'id', autoIncrement: true });
            }
            if (!db.objectStoreNames.contains('appointments')) {
                db.createObjectStore('appointments', { keyPath: 'id', autoIncrement: true });
            }
        };
    });
};

window.saveToIndexedDB = async (dbName, storeName, data) => {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(dbName);
        
        request.onerror = () => reject(request.error);
        
        request.onsuccess = (event) => {
            const db = event.target.result;
            const transaction = db.transaction([storeName], 'readwrite');
            const store = transaction.objectStore(storeName);
            
            const jsonData = JSON.parse(data);
            const saveRequest = store.put(jsonData);
            
            saveRequest.onsuccess = () => resolve();
            saveRequest.onerror = () => reject(saveRequest.error);
        };
    });
};

window.getFromIndexedDB = async (dbName, storeName, id) => {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(dbName);
        
        request.onerror = () => reject(request.error);
        
        request.onsuccess = (event) => {
            const db = event.target.result;
            const transaction = db.transaction([storeName], 'readonly');
            const store = transaction.objectStore(storeName);
            
            const getRequest = store.get(id);
            
            getRequest.onsuccess = () => resolve(JSON.stringify(getRequest.result));
            getRequest.onerror = () => reject(getRequest.error);
        };
    });
};

window.getAllFromIndexedDB = async (dbName, storeName) => {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(dbName);
        
        request.onerror = () => reject(request.error);
        
        request.onsuccess = (event) => {
            const db = event.target.result;
            const transaction = db.transaction([storeName], 'readonly');
            const store = transaction.objectStore(storeName);
            const getAllRequest = store.getAll();
            
            getAllRequest.onsuccess = () => {
                resolve(getAllRequest.result.map(item => JSON.stringify(item)));
            };
            getAllRequest.onerror = () => reject(getAllRequest.error);
        };
    });
};

window.deleteFromIndexedDB = async (dbName, storeName, id) => {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(dbName);
        
        request.onerror = () => reject(request.error);
        
        request.onsuccess = (event) => {
            const db = event.target.result;
            const transaction = db.transaction([storeName], 'readwrite');
            const store = transaction.objectStore(storeName);
            
            const deleteRequest = store.delete(id);
            
            deleteRequest.onsuccess = () => resolve();
            deleteRequest.onerror = () => reject(deleteRequest.error);
        };
    });
};