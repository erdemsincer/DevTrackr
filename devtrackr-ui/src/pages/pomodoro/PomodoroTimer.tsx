import { useEffect, useState } from "react";

interface PomodoroTimerProps {
    sessionId: number;
    focusMinutes: number;
    onComplete: (id: number) => void;
}

const PomodoroTimer = ({ sessionId, focusMinutes, onComplete }: PomodoroTimerProps) => {
    const [secondsLeft, setSecondsLeft] = useState(focusMinutes * 60);

    useEffect(() => {
        if (secondsLeft <= 0) {
            onComplete(sessionId); // otomatik tamamlama
            return;
        }

        const timer = setInterval(() => {
            setSecondsLeft((prev) => prev - 1);
        }, 1000);

        return () => clearInterval(timer);
    }, [secondsLeft, sessionId, onComplete]);

    const minutes = Math.floor(secondsLeft / 60);
    const seconds = secondsLeft % 60;

    return (
        <div className="pomodoro-timer">
            <h2>⏳ Aktif Pomodoro</h2>
            <div className="time-display">
                {String(minutes).padStart(2, "0")}:
                {String(seconds).padStart(2, "0")}
            </div>
        </div>
    );
};

export default PomodoroTimer;
