const dbName = 'HospitalAppointmentSystem';
const dbVersion = 1;

export const initDatabase = async () => {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open(dbName, dbVersion);

        request.onerror = () => reject(request.error);
        request.onsuccess = () => resolve(request.result);

        request.onupgradeneeded = (event) => {
            const db = event.target.result;

            // Create stores based on your existing models
            if (!db.objectStoreNames.contains('appointments')) {
                const appointmentStore = db.createObjectStore('appointments', { keyPath: 'id', autoIncrement: true });
                appointmentStore.createIndex('doctorId', 'doctorId');
                appointmentStore.createIndex('patientId', 'patientId');
                appointmentStore.createIndex('date', 'appointmentDate');
            }

            if (!db.objectStoreNames.contains('doctors')) {
                const doctorStore = db.createObjectStore('doctors', { keyPath: 'id', autoIncrement: true });
                doctorStore.createIndex('name', 'name');
            }

            if (!db.objectStoreNames.contains('patients')) {
                db.createObjectStore('patients', { keyPath: 'id', autoIncrement: true });
            }

            if (!db.objectStoreNames.contains('users')) {
                const userStore = db.createObjectStore('users', { keyPath: 'id', autoIncrement: true });
                userStore.createIndex('email', 'email', { unique: true });
            }

            if (!db.objectStoreNames.contains('medicalRecords')) {
                const recordsStore = db.createObjectStore('medicalRecords', { keyPath: 'id', autoIncrement: true });
                recordsStore.createIndex('patientId', 'patientId');
            }
        };
    });
};