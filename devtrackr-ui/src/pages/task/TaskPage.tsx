import { useEffect, useState } from "react";
import axios from "axios";
import { useAuth } from "../../context/AuthContext";
import "./TaskPage.css";

interface Task {
    id: number;
    title: string;
    description?: string;
    isCompleted: boolean;
    createdAt: string;
    completedAt?: string;
}

const TaskPage = () => {
    const { token } = useAuth();
    const [tasks, setTasks] = useState<Task[]>([]);
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");

    const api = axios.create({
        baseURL: "http://localhost:5103/api/Task",
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });

    const fetchTasks = async () => {
        const res = await api.get("/");
        setTasks(res.data);
    };

    const createTask = async () => {
        if (!title.trim()) return;
        await api.post("/", { title, description });
        setTitle("");
        setDescription("");
        fetchTasks();
    };

    const toggleComplete = async (task: Task) => {
        await api.put(`/${task.id}`, {
            title: task.title,
            description: task.description,
            isCompleted: !task.isCompleted,
        });
        fetchTasks();
    };

    const deleteTask = async (id: number) => {
        await api.delete(`/${id}`);
        fetchTasks();
    };

    useEffect(() => {
        if (token) fetchTasks();
    }, [token]);

    return (
        <div className="task-container">
            <h1 className="page-title">✅ Görevlerim</h1>

            <div className="task-form">
                <input
                    type="text"
                    placeholder="Görev başlığı"
                    value={title}
                    onChange={(e) => setTitle(e.target.value)}
                />
                <input
                    type="text"
                    placeholder="Açıklama (opsiyonel)"
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                />
                <button onClick={createTask}>Görev Ekle</button>
            </div>

            <div className="task-list">
                {tasks.map((task) => (
                    <div key={task.id} className={`task-card ${task.isCompleted ? "done" : ""}`}>
                        <div className="task-info">
                            <strong>{task.title}</strong>
                            {task.description && <p>{task.description}</p>}
                            <p>Durum: {task.isCompleted ? "✅ Tamamlandı" : "⏳ Bekliyor"}</p>
                        </div>
                        <div className="task-actions">
                            <button onClick={() => toggleComplete(task)}>
                                {task.isCompleted ? "Geri Al" : "Tamamla"}
                            </button>
                            <button onClick={() => deleteTask(task.id)} className="delete-btn">
                                Sil
                            </button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default TaskPage;
