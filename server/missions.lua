local missionClient

RegisterServerEvent("freeroam:missionrunning")
AddEventHandler("freeroam:missionrunning", function(source, state)
	if not state then
		missionClient = nil
	else
		missionClient = source
	end

	TriggerClientEvent("freeroam:missionrunning", -1, GetPlayerName(source), state)
end)

AddEventHandler("playerDropped", function(reason)
	if source == missionClient then
		missionClient = nil
		TriggerClientEvent("freeroam:missionrunning", -1, false)
	end
end)